using System.Text.RegularExpressions;
using System.Net;

namespace ChestCounter;

/// <summary>Rarity of the items that came out of a chest. Item ids end with the rarity: WID_..._Athena_UC, WID_..._VR_Ore_T03.</summary>
public static class Loot
{
    private static readonly Regex RarityToken = new(@"(?:^|_)(UR|SR|VR|UC|R|C)(?=_|$)", RegexOptions.Compiled);
    private static readonly Regex TierToken = new(@"^T\d\d$", RegexOptions.Compiled);

    /// <summary>"c", "uc", "r", "vr", "sr" or "ur"; null for items that have no rarity (ammo, bandages, currency).</summary>
    public static string? RarityCode(string rawItem)
    {
        var m = RarityToken.Match(rawItem);
        return m.Success ? m.Groups[1].Value.ToLowerInvariant() : null;
    }

    /// <summary>Order for sorting, rarest first.</summary>
    public static int Rank(string code) => code switch { "ur" => 6, "sr" => 5, "vr" => 4, "r" => 3, "uc" => 2, _ => 1 };

    public static string RarityName(string code) => code switch
    {
        "ur" => L.T("Mythique", "Mythic", "Mítico"),
        "sr" => L.T("Légendaire", "Legendary", "Legendario"),
        "vr" => L.T("Épique", "Epic", "Épico"),
        "r" => L.T("Rare", "Rare", "Raro"),
        "uc" => L.T("Peu commun", "Uncommon", "Poco común"),
        _ => L.T("Commun", "Common", "Común"),
    };

    /// <summary>Readable item name without the rarity and tier tokens ("Hook Gun Ore").</summary>
    public static string Name(string rawItem)
    {
        var code = RarityCode(rawItem);
        var parts = Pretty.ItemName(rawItem).Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        if (code != null)
        {
            var i = parts.FindIndex(p => string.Equals(p, code, StringComparison.OrdinalIgnoreCase));
            if (i >= 0)
            {
                parts.RemoveAt(i);
            }
        }
        parts.RemoveAll(p => TierToken.IsMatch(p));
        return parts.Count > 0 ? string.Join(' ', parts) : rawItem;
    }

    /// <summary>One item as HTML: a colored dot for its rarity and its name.</summary>
    public static string Chip(string rawItem)
    {
        var name = WebUtility.HtmlEncode(Name(rawItem));
        var code = RarityCode(rawItem);
        return code == null
            ? $"<span class=\"loot muted\">{name}</span>"
            : $"<span class=\"loot\"><i class=\"dot r-{code}\" title=\"{RarityName(code)}\"></i>{name}</span>";
    }

    /// <summary>"● Epic ×2 · ● Uncommon ×4", rarest first; empty when no item has a rarity.</summary>
    public static string Summary(IEnumerable<string> rawItems)
    {
        var groups = rawItems.Select(RarityCode).Where(c => c != null).GroupBy(c => c!)
            .OrderByDescending(g => Rank(g.Key)).ToList();
        return string.Join(" &nbsp;·&nbsp; ", groups.Select(g => $"<span class=\"loot\"><i class=\"dot r-{g.Key}\"></i>{RarityName(g.Key)} ×{g.Count()}</span>"));
    }
}
