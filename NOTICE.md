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

The counting logic, the parser (`lib/`) and the overlay are unchanged.
