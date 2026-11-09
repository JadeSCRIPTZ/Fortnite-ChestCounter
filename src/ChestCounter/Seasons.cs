using System.Text.RegularExpressions;

namespace ChestCounter;

/// <summary>
/// Maps the game version recorded in a replay ("++Fortnite+Release-42.00") to its season. The major version is the
/// season counter since Chapter 1 Season 1 (v21 = Chapter 3 Season 3, v42 = Chapter 7 Season 4).
/// </summary>
public static class Seasons
{
    private static readonly Regex VersionPattern = new(@"Release-(\d+)\.(\d+)", RegexOptions.Compiled);

    public static int? Major(string? gameVersion)
    {
        var m = gameVersion == null ? null : VersionPattern.Match(gameVersion);
        return m is { Success: true } && int.TryParse(m.Groups[1].Value, out var major) ? major : null;
    }

    private static string Chapter(int c) => L.T($"Chapitre {c}", $"Chapter {c}", $"Capítulo {c}");
    private static string Season(string s) => L.T($"Saison {s}", $"Season {s}", $"Temporada {s}");
    private static string Mini(int n) => L.T($"Mini-saison {n}", $"Mini Season {n}", $"Minitemporada {n}");

    /// <summary>"Chapter 7 · Season 4" for a known version, "Season v44" for one newer than this table.</summary>
    public static string Label(int? major) => major switch
    {
        null => L.T("Version inconnue", "Unknown version", "Versión desconocida"),
        <= 0 => L.T("Version inconnue", "Unknown version", "Versión desconocida"),
        <= 9 => $"{Chapter(1)} · {Season(major.Value.ToString())}",
        10 => $"{Chapter(1)} · {Season("X")}",
        <= 18 => $"{Chapter(2)} · {Season((major.Value - 10).ToString())}",
        <= 22 => $"{Chapter(3)} · {Season((major.Value - 18).ToString())}",
        <= 26 => $"{Chapter(4)} · {Season((major.Value - 22).ToString())}",
        27 => $"{Chapter(4)} · OG",
        <= 31 => $"{Chapter(5)} · {Season((major.Value - 27).ToString())}",
        32 => $"{Chapter(2)} · Remix",
        33 => $"{Chapter(6)} · {Season("1")}",
        34 => $"{Chapter(6)} · {Season("2")}",
        35 => $"{Chapter(6)} · {Mini(1)}",
        36 => $"{Chapter(6)} · {Season("3")}",
        37 => $"{Chapter(6)} · {Season("4")}",
        38 => $"{Chapter(6)} · {Mini(2)}",
        <= 42 => $"{Chapter(7)} · {Season((major.Value - 38).ToString())}",
        43 => $"{Chapter(7)} · {L.T("Mini-saison", "Mini Season", "Minitemporada")}",
        _ => L.T($"Saison v{major}", $"Season v{major}", $"Temporada v{major}"),
    };
}
