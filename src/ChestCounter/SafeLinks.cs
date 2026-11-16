namespace ChestCounter;

/// <summary>
/// What the counter's window may open outside itself. The page shows text that comes from outside (notices of content.json,
/// names of players in the replays): a link in it is only ever opened in the player's browser, and only when it is a plain
/// web address. Never a file, a program or a system protocol (file:, ms-msdt:, search-ms: ...).
/// </summary>
public static class SafeLinks
{
    /// <summary>An absolute http(s) address, without a user name or password in it.</summary>
    public static bool IsWebLink(string? url) =>
        url is { Length: > 0 and <= 2000 }
        && Uri.TryCreate(url, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
        && string.IsNullOrEmpty(uri.UserInfo)
        && uri.Host.Length > 0;

    /// <summary>True for the counter's own page (same local address and port as <paramref name="ownUrl"/>) and for a blank page.</summary>
    public static bool IsOwnPage(string? url, string ownUrl)
    {
        if (url == "about:blank")
        {
            return true;
        }
        return Uri.TryCreate(url, UriKind.Absolute, out var uri) && Uri.TryCreate(ownUrl, UriKind.Absolute, out var own)
            && uri.Scheme == own.Scheme && uri.Host == own.Host && uri.Port == own.Port;
    }
}
