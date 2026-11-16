# Fortnite Chest Counter (fork cu verificare de securitate)

<p align="center"><img src="docs/home-en.png" height="190" alt="fereastra"> <img src="docs/overlay-en.png" height="190" alt="overlay"></p>

Numără cuferele pe care le deschizi (sau le spargi) în Fortnite, meci după meci, **citind replay-urile** pe care Fortnite le salvează singur,
și le arată într-o fereastră proprie, într-un overlay peste joc și într-un raport (și ca fișier HTML).

## De ce nu îți riscă contul

- Citește doar fișierele `.replay` din `%LOCALAPPDATA%\FortniteGame\Saved\Demos`, **doar pentru citire**, după ce se termină meciul.
- **Nu atinge procesul jocului**: nu citește memoria, nu injectează nimic, nu modifică niciun fișier din instalarea Fortnite.
- **Nu atinge contul Epic**: nu are login, token-uri, cookies, nu se uită în folderul Epic Games Launcher.
- **Fără rețea implicit**: verificarea de actualizări e dezactivată din fabrică (o pornești din **Setări** dacă vrei). Imaginile comorilor sunt incluse în exe, deci nu se descarcă nimic.
- Datele rămân local, în `%APPDATA%\ChestCounter`.

Singurul lucru pe care îl face în legătură cu jocul rulat este să verifice dacă procesul `FortniteClient-Win64-Shipping` există în lista de procese (doar pentru opțiunea „Open with Fortnite”, care e opțională).

## Cum îl folosești

1. În Fortnite: *Settings → Game → Replays → Record Replays: **On***.
2. Rulează `ChestCounter.exe` (Windows poate spune „Windows protected your PC”, fiindcă exe-ul nu e semnat: *More info → Run anyway*).
3. Pentru overlay peste joc, pune Fortnite pe **Windowed Fullscreen**.
4. Fereastra are taburi: **Acasă**, **Raport**, **Overlay**, **Setări**, **Ajutor**. Scurtături globale pentru overlay: `Ctrl+Shift+O` îl arată/ascunde, `Ctrl+Shift+L` îl blochează/deblochează.

Cuferele se adaugă la câteva secunde după fiecare meci, nu în timpul lui (Fortnite scrie cheia replay-ului abia la final).

## Detaliu per meci: tu, echipa și ceilalți jucători

În raport (tab-ul **Report**), la **Latest matches**, dă click pe orice meci. Se deschide:

- **Echipa ta**: câte cufere ai deschis tu și fiecare coechipier, câte au fost sparte, ce tipuri, plus totalul echipei.
- **Ceilalți jucători văzuți în replay**: cine a deschis câte cufere în meciul respectiv.
- **Cufere tale**: minutul, tipul și ce a ieșit din fiecare.

În lista de meciuri, coloana de echipă (comutatorul *Show the squad total*) arată chiar în listă cuferele coechipierilor.

**Limită importantă:** replay-ul e înregistrat de jocul *tău*, deci conține doar ce a văzut jocul tău. Cuferele deschise departe de tine pot lipsi, așa că
numerele celorlalți jucători (și uneori ale coechipierilor care s-au despărțit de tine) sunt **un minim**, nu un total exact. Numărul tău e cel exact.

La prima pornire cu o versiune nouă, replay-urile rămase pe disc se re-analizează automat ca să primești detaliile și pentru meciurile vechi.
Meciurile al căror replay a fost șters rămân în istoric, dar fără detaliul pe jucători.
Numele celorlalți jucători (cele afișate în joc) se salvează doar local, în `data.json`, și nu pleacă nicăieri.

## Ce aduce v1.7.0

Fuziune cu **Chest Counter 1.5.1 de LED-N** (am citit tot codul nou înainte; nu conține malware): aplicația se deschide acum **într-o fereastră proprie**
(Acasă, Raport, Overlay, Setări, Ajutor) în loc de consolă, cu secțiunea **Your chances** (șansele comorilor săptămânale, numărătoare până la
următoarea Treasure Hour), colecția de comori cu imagini, cufere adăugate manual și ore locale/UTC.

**Important:** de la 1.5 încoace se numără doar **Battle Royale obișnuit pe harta sezonului curent**. Ranked, OG, Reload, Blitz și celelalte moduri
nu se mai numără (consola/jurnalul spun de ce un meci nu a fost numărat).

Peste ele, acest fork a întărit securitatea (linkurile din fereastră se deschid doar ca adrese web, nicio notiță nu poate rula cod, **zero acces la rețea
dacă verificarea de update e oprită**, imaginile comorilor sunt incluse în exe, dezinstalarea șterge doar folderul propriu) și păstrează funcțiile lui:
detaliu pe meci cu ceilalți jucători, sezoane, moduri, căutare de jucători, raritate, `players.csv`.

## Ce a adus v1.6.0

Fuziune cu **Chest Counter 1.4.0 de LED-N**: overlay HUD transparent, consolă nouă, icon în tray, raport „logbook" și **numărătoare corectată**
(cufere reumplute, cufere îngropate/relicve, variante numerotate).

## Sezoane, căutare de jucători și calitatea loot-ului

În raport:

- **Par saison / By season**: cufere pe sezon (din versiunea jocului din fiecare replay: v42 = Chapter 7 Season 4, v32 = Chapter 2 Remix).
- **Par mode / By mode** și **Avec qui tu joues / Who you play with** (coechipierii tăi cei mai frecvenți și câte cufere deschid).
- **Chercher un joueur / Find a player**: scrii un nume și vezi toate meciurile în care a apărut, cu câte cufere a deschis. Click pe un rezultat deschide meciul.
- **Raritate**: punct colorat pentru ce a ieșit din cufere (gri comun, verde neobișnuit, albastru rar, mov epic, auriu legendar).
- **`players.csv`** în `%APPDATA%\ChestCounter`: un rând per jucător per meci, pentru Excel/Sheets.

## Versiuni

Toate versiunile rămân în [Releases](../../releases), fiecare cu propriul `ChestCounter.exe` și SHA-256-ul lui în descriere.
Ce aduce fiecare versiune e în [`CHANGELOG.md`](CHANGELOG.md). Dacă o versiune nouă nu-ți place, descarci pur și simplu cea veche.

## Build

GitHub Actions rulează testele (`tests/SelfTest`), construiește `ChestCounter.exe` la fiecare push (artefact `ChestCounter`), iar la un tag `v*` îl atașează la Release.
Local: instalează .NET 10 SDK și rulează `build.bat`.

Creditele: [LED-N](https://github.com/LED-N/Fortnite-ChestCounter) (aplicația originală, MIT), [Shiqan](https://github.com/Shiqan/FortniteReplayDecompressor) (parserul de replay, MIT), koiseka (contribuție #6 la originalul).

Vezi [`NOTICE.md`](NOTICE.md) pentru credite și lista modificărilor față de original.
