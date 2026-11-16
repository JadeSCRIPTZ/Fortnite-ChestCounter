# Changelog

Every version stays available in [Releases](../../releases), each with its own `ChestCounter.exe` and its SHA-256.

## 1.7.0 — 2026-11-16
- **Merged with LED-N/Fortnite-ChestCounter 1.5.1** (reviewed before merging; no malware found): the counter opens in its own window
  (Windows Forms + WebView2, served by a local server on 127.0.0.1 with a random token), "Your chances", Treasure Hour countdown,
  notices, treasure collection, chests added by hand, local/UTC time, overlay improvements, chests broken without touching them counted
  again, matches counted only for regular Battle Royale on the season's map. Thanks to koiseka and tbpg58 for their upstream contributions.
- **Hardened on top of it** (weak points found in the review, none is malicious):
  - a notice's link and id from `content.json` can no longer become JavaScript in the page (data attributes, https only);
  - the window opens only plain web addresses (never a file, a program or a system protocol) and never leaves its own page;
  - no network access unless the update check is on (upstream fetched `content.json` and the pictures even with updates off);
    the treasure pictures are built into the exe;
  - `content.json` comes from this repository only (no environment override); pictures only from this repository or the picture host named in it;
  - the development hooks that wrote the window's address (with its token) to a file are removed;
  - the uninstaller deletes the data folder only when it holds the counter's own files (never a drive root or a foreign folder).
- Kept from this fork: update check off by default, updater limited to this repository's releases and to releases with a SHA-256,
  no third-party GitHub Action, match detail with the other players, by season / by mode / who you play with, player search,
  loot rarity, `players.csv`, `tests/SelfTest` (46 checks).
- Analyzer version 18: replays still on disk are re-analyzed once. The teammates' numbers of the match detail match upstream's own count.

## 1.6.0 — 2026-11-10
- **Merged with LED-N/Fortnite-ChestCounter 1.4.0** (audited before merging; no new network, process, registry or memory access):
  transparent HUD overlay, new console, tray icon, "logbook" report design, team treasures (estimate), analyzer fixes
  (refilled chests counted again, buried/relic chests no longer counted twice, chests with numbered variants, actors whose
  export group arrives late). Thanks to koiseka (#6) for their upstream contribution.
- Kept from this fork: update check off by default, updater limited to this repository's releases and to releases with a SHA-256,
  no third-party GitHub Action, match detail, seasons, modes, teammates, player search, loot rarity, `players.csv`.
- The player detail follows the new counting rules (stale searches are not counted for other players either).
- Analyzer version 16: replays still on disk are re-analyzed once.
- Release notes of each version are built into the exe ("What's new" after an update) and are the text of its release.

## 1.5.0 — 2026-11-09
- Report: **By season**, **By mode**, **Who you play with** tables.
- Report: **Find a player** search over every player recorded in your matches (click a result to open the match).
- Report: loot rarity dots on the contents of your chests and a rarity summary per match.
- New `players.csv` (one row per player per match; names quoted, formulas neutralized).
- `tests/SelfTest` extended (seasons, rarity, hostile player names in HTML/JSON/CSV).
- Release notes are written per version and include the SHA-256 of the exe.

## 1.4.0 — 2026-11-09
- Match detail in the report (you, your team, other players seen in the replay, your own chests with their contents).
- Team total in the matches table, in the console and in `matches.csv`.
- The report no longer reloads while a match is open.
- Analyzer version 11: replays still on disk are re-analyzed once.
- `tests/SelfTest` run by CI before each build.

## 1.3.0 — 2026-11-09
- Hardened fork of LED-N/Fortnite-ChestCounter v1.3.0 (commit `daba16d`): own repository, update check off by default,
  updater limited to this repository's releases and to releases with a SHA-256, no third-party GitHub Action.
