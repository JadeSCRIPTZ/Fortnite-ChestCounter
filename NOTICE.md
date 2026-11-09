# Credits

This project is a hardened fork of **Fortnite Chest Counter** by LED-N
(https://github.com/LED-N/Fortnite-ChestCounter, MIT), reviewed at commit `daba16d` (v1.3.0).
Replay parsing comes from **FortniteReplayDecompressor** by Shiqan (https://github.com/Shiqan/FortniteReplayDecompressor, MIT),
see `lib/PATCHES.md` and `lib/LICENSE-FortniteReplayDecompressor`.

Not affiliated with, endorsed by or sponsored by Epic Games. Fortnite is a trademark of Epic Games, Inc.

## Changes compared with the original

1. `AppInfo.cs`: the repository is `JadeSCRIPTZ/Fortnite-ChestCounter`, so the exe never talks to the original author's repository.
2. `Models.cs`: update checking is **off by default** (no network access at all until you turn it on with `C`).
3. `Updater.cs`: fixed endpoint (the `CHESTCOUNTER_UPDATE_API` override is removed); the exe is accepted only from this
   repository's own `releases/download/` URL; an update with no SHA-256 digest is refused.
4. `.github/workflows/build.yml`: no third-party action (the GitHub CLI attaches the exe), read-only default permissions,
   SHA-256 of the exe printed in the run summary.
5. Original screenshots and translated READMEs removed (they pointed to the original release downloads).

## Additions (v1.4.0)

6. **Match detail**: the report shows, per match, the chests of you, your teammates and the other players seen in the replay
   (`PlayerChestStats`, `ReplayAnalyzer.BuildRoster`), plus your own chests with their contents. The original already recorded every
   player's chest searches but discarded all but the tracked player's. Your own count is built from the same data as before.
7. Team total in the matches table, in the console after each match and in `matches.csv` (`team_chests`, appended as the last column).
8. The report no longer reloads while a match detail is open; the open details survive the automatic reload.
9. `tests/SelfTest`, run by CI before each build.

The counting logic, the parser (`lib/`) and the overlay are unchanged.
