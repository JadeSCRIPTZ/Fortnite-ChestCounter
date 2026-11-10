# Changelog

Every version stays available in [Releases](../../releases), each with its own `ChestCounter.exe` and its SHA-256.

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
