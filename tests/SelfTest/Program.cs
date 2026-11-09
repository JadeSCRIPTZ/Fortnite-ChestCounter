using System.Reflection;
using ChestCounter;

// Small self-checks run by CI before every build: the hardened updater and the match detail of the report.
// (Replay parsing itself is not tested here: real replays contain other people's names, so none is committed.)
L.Set("en");
var fail = 0;
void Check(string name, bool ok) { Console.WriteLine((ok ? "PASS " : "FAIL ") + name); if (!ok) fail++; }

// ---------- updater ----------
var parse = typeof(Updater).GetMethod("Parse", BindingFlags.NonPublic | BindingFlags.Static)!;
string Release(string tag, string url, string digest) => $$"""
    {"tag_name":"{{tag}}","draft":false,"prerelease":false,"html_url":"https://github.com/x","assets":[{"name":"ChestCounter.exe","size":123,"digest":{{digest}},"browser_download_url":"{{url}}"}]}
    """;
object Parse(string json) => parse.Invoke(null, new object[] { json });
var own = $"https://github.com/{AppInfo.Repo}/releases/download/v99.0.0/ChestCounter.exe";
Check("repo is JadeSCRIPTZ/Fortnite-ChestCounter", AppInfo.Repo == "JadeSCRIPTZ/Fortnite-ChestCounter");
Check("newer release from own repo accepted", Parse(Release("v99.0.0", own, "\"sha256:abcd\"")) != null);
Check("asset from the original author's repo rejected", Parse(Release("v99.0.0", own.Replace("JadeSCRIPTZ", "LED-N"), "\"sha256:abcd\"")) == null);
Check("asset from another host rejected", Parse(Release("v99.0.0", "https://evil.example/ChestCounter.exe", "\"sha256:abcd\"")) == null);
Check("look-alike repository name rejected", Parse(Release("v99.0.0", $"https://github.com/{AppInfo.Repo}-evil/releases/download/v99/ChestCounter.exe", "\"sha256:abcd\"")) == null);
Check("same or older version ignored", Parse(Release("v0.0.1", own, "\"sha256:abcd\"")) == null);
Check("update check is off by default", new Config().UpdateMode == "off");
Environment.SetEnvironmentVariable("CHESTCOUNTER_UPDATE_API", "https://evil.example/api");
var api = (string)typeof(Updater).GetProperty("ApiUrl", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null);
Check("environment variable cannot redirect the update endpoint", api.StartsWith("https://api.github.com/repos/JadeSCRIPTZ/"));

// ---------- match detail in the report ----------
PlayerChestStats P(string name, int chests, bool me = false, bool team = false, int destroyed = 0) => new()
{
    Name = name, IsMe = me, IsTeammate = team, ChestsOpened = chests, ChestsDestroyed = destroyed,
    Kinds = chests > 0 ? new() { [ChestKinds.Normal] = chests } : new(),
};
var evil = "<img src=x onerror=alert(1)>";
var match = new MatchRecord
{
    SessionId = "t-1", ReplayFile = "t.replay", PlayerName = "Me", EpicId = "e1", MatchStartUtc = DateTime.UtcNow.AddHours(-1),
    Playlist = "Playlist_DefaultSquad", GameVersion = "++Fortnite+Release-42.00", Placement = 3, TotalPlayers = 100, ChestsOpened = 5,
    Chests = { new ChestRecord { TimeSeconds = 90, Kind = ChestKinds.Normal, Items = { "WID_Shotgun_Standard_Athena_UC", "WID_Hook_Gun_VR_Ore_T03", "WadsItemData" } } },
    Players = new()
    {
        P("Me", 5, me: true), P("Mate1", 13, team: true, destroyed: 2), P("Mate2", 0, team: true),
        P(evil, 3), P("Other", 2), P("</script><script>alert(2)</script>", 1), P("=HYPERLINK(\"http://x\")", 1), P("semi;colon", 1),
    },
};
Check("TeamChests = you + teammates", match.TeamChests == 18);
var config = new Config();
var data = new TrackerData(); data.Matches.Add(match);
var html = ReportWriter.Report(config, Profiles.View(data, config), Profiles.List(data));
Check("detail shows the team total", html.Contains("<b>18 chests</b>"));
Check("detail lists teammates and the other players", html.Contains("Mate1") && html.Contains("Mate2") && html.Contains("Other"));
Check("a player name can not inject HTML", !html.Contains(evil) && html.Contains("&lt;img src=x onerror=alert(1)&gt;"));
Check("item names are cleaned (no ItemData suffix)", html.Contains("Wads") && !html.Contains("WadsItemData"));
Check("script keeps the details open across reloads", html.Contains("sessionStorage") && html.Contains("<noscript>"));

// ---------- seasons ----------
Check("version string -> season number", Seasons.Major("++Fortnite+Release-42.00") == 42 && Seasons.Major("++Fortnite+Release-11.31") == 11 && Seasons.Major(null) == null);
Check("v42 = Chapter 7 Season 4", Seasons.Label(42) == "Chapter 7 · Season 4");
Check("v21 = Chapter 3 Season 3", Seasons.Label(21) == "Chapter 3 · Season 3");
Check("v32 = Chapter 2 Remix, v11 = Chapter 2 Season 1", Seasons.Label(32) == "Chapter 2 · Remix" && Seasons.Label(11) == "Chapter 2 · Season 1");
Check("a version newer than the table still gets a label", Seasons.Label(99) == "Season v99");

// ---------- loot rarity ----------
Check("rarity from item ids", Loot.RarityCode("WID_Hook_Gun_VR_Ore_T03") == "vr" && Loot.RarityCode("WID_Shotgun_MusterPump_Athena_UC") == "uc" && Loot.RarityCode("Athena_SilverBlazer_Mini_UC") == "uc");
Check("items without rarity have none", Loot.RarityCode("WID_Paprika_Bandage") == null && Loot.RarityCode("AthenaAmmoDataBulletsLight") == null && Loot.RarityCode("Athena_WadsItemData") == null);
Check("item names lose rarity and tier tokens", Loot.Name("WID_Hook_Gun_VR_Ore_T03") == "Hook Gun Ore" && Loot.Name("WID_Shotgun_MusterPump_Athena_UC") == "Shotgun MusterPump");
Check("the report shows rarity dots", html.Contains("r-vr") && html.Contains("Epic"));

// ---------- seasons / mode / teammates / search in the report ----------
Check("report has the season table", html.Contains("By season") && html.Contains("Chapter 7 &#183; Season 4") && html.Contains(">v42<"));
Check("report has the teammates table", html.Contains("Who you play with") && html.Contains("Mate1"));
Check("report has the player search", html.Contains("id=\"psearch\"") && html.Contains("id=\"pdata\""));
Check("a name can not close the embedded data script", !html.Contains("</script><script>alert(2)"));
Check("hostile names are escaped in the embedded data", html.Contains("\\u003C/script\\u003E"));

// ---------- players.csv ----------
var pcsv = ReportWriter.PlayersCsv(data);
var plines = pcsv.Split('\n', StringSplitOptions.RemoveEmptyEntries);
Check("players.csv header and one row per player", plines[0].StartsWith("date;mode;file;player;role;chests") && plines.Length == 1 + match.Players.Count);
Check("players.csv neutralizes formulas and quotes separators", pcsv.Contains("\"'=HYPERLINK(\"\"http://x\"\")\"") && pcsv.Contains("\"semi;colon\""));
Check("players.csv roles", pcsv.Contains(";you;5;") && pcsv.Contains(";team;13;") && pcsv.Contains(";other;2;"));

var csv = ReportWriter.Csv(data);
Check("csv has the team column", csv.Split('\n')[0].TrimEnd().EndsWith("team_chests") && csv.Contains(";18"));
var old = new MatchRecord { SessionId = "t-0", ReplayFile = "o.replay", PlayerName = "Me", EpicId = "e1", MatchStartUtc = DateTime.UtcNow.AddDays(-3), ChestsOpened = 1 };
data.Matches.Add(old);
var html2 = ReportWriter.Report(config, Profiles.View(data, config), Profiles.List(data));
Check("a match without player data still renders", html2.Contains("Player details not available"));

Console.WriteLine(fail == 0 ? "ALL OK" : $"{fail} FAILED");
return fail;
