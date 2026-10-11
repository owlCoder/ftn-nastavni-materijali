# Nastavni materijali — FTN

Praktikumi, zadaci, prezentacije, projektne specifikacije i programski primeri za Primenjeno softversko inženjerstvo.

| Predmet | Praktikum i zadaci | Prezentacije | Projekat | Primeri |
|---|---|---|---|---|
| Elementi razvoja softvera | [ERS](<01 Elementi Razvoja Softvera/03 Racunarske vezbe/Praktikum.md>) | [PDF i PPTX](<01 Elementi Razvoja Softvera/02 Prezentacije>) | [Specifikacija](<01 Elementi Razvoja Softvera/01 Projekat/ERS_Projektna_Specifikacija.pdf>) | [Kod](<01 Elementi Razvoja Softvera/04 Primeri>) |
| Osnove informacione bezbednosti | [OIB](<02 Osnove Informacione Bezbednosti/03 Racunarske vezbe/Praktikum.md>) | [PDF i PPTX](<02 Osnove Informacione Bezbednosti/02 Prezentacije>) | [Specifikacija](<02 Osnove Informacione Bezbednosti/01 Projekat/OIB_Projektna_Specifikacija.pdf>) | [Kod](<02 Osnove Informacione Bezbednosti/04 Primeri>) |
| Osnove distribuiranog programiranja | [ODP](<03 Osnove Distribuiranog Programiranja/03 Racunarske vezbe/Praktikum.md>) | [PDF i PPTX](<03 Osnove Distribuiranog Programiranja/02 Prezentacije>) | [Specifikacija](<03 Osnove Distribuiranog Programiranja/01 Projekat/ODP_Specifikacija.pdf>) | [Kod](<03 Osnove Distribuiranog Programiranja/04 Primeri>) |
| Informaciona bezbednost infrastrukturnih sistema | — | [PDF i PPTX](<04 Informaciona Bezbednost Infrastrukturnih Sistema/01 Prezentacije>) | [SCADALattice, RackMetria, RouteMetria](<04 Informaciona Bezbednost Infrastrukturnih Sistema/02 Projekti>) | — |

Zadaci i primeri odgovaraju [nastavnom portalu](https://owlcoder.github.io/ftn-nastavni-portal/). U `03 Racunarske vezbe` nalaze se tekstovi osam pojedinačnih vežbi, ceo praktikum i kontrolne tačke. PDF datoteke služe za čitanje; PPTX i Markdown izvori za uređivanje. Obim projekta i pravila polaganja određuju dokumenti predmeta.

## Zajednička uputstva

[Tapiz LMS](<00 Opsti Dokumenti/PDF/Uputstvo_Tapiz_LMS.pdf>) · [Tapiz Boards](<00 Opsti Dokumenti/PDF/Tapiz_Boards_Uputstvo.pdf>) · [Kova](<00 Opsti Dokumenti/PDF/Kova-Uputstvo.pdf>) · [Toren](<00 Opsti Dokumenti/PDF/Toren-Uputstvo.pdf>)

## Programski primeri

Potreban je .NET 8 SDK. Komande izvršavati iz direktorijuma izabranog primera.

| ERS primer | Solution | Pokretanje |
|---|---|---|
| [Logger–Blogger, vežba 2](<01 Elementi Razvoja Softvera/04 Primeri/02 Logger Blogger>) | `Loger-Bloger.sln` | `dotnet run --project Presentation` |
| [ECommerce, vežba 3](<01 Elementi Razvoja Softvera/04 Primeri/03 ECommerce>) | `ECommerce.sln` | `dotnet run --project WebAPI` |
| [Equipment Reservation, vežbe 5–8](<01 Elementi Razvoja Softvera/04 Primeri/ers-ai-workflow>) | `EquipmentReservation.sln` | `dotnet run --project src/EquipmentReservation.ConsoleUi` ili `src/EquipmentReservation.Api` |

```bash
dotnet build <naziv-solutiona>.sln --configuration Release
dotnet test <naziv-solutiona>.sln --configuration Release --no-build
```

Logger–Blogger koristi demo naloge `maja` / `123` i `ana` / `123`. ECommerce i Equipment Reservation ispisuju adresu API-ja pri pokretanju. Adapteri za podatke su namenjeni nastavnim demonstracijama.

Za OIB i ODP koristiti broj vežbe `NN` od `01` do `08`:

```bash
dotnet run --project src/Oib.VezbaNN.ConsoleUi
dotnet test Oib.VezbaNN.sln --configuration Release
# ODP: zameniti Oib sa Odp u obe komande.
```

Scenario, očekivani dokaz i zadatak uz svaki primer navedeni su u odgovarajućem poglavlju praktikuma.

## ERS — AI razvojni tok

U Equipment Reservation primeru `AGENTS.md` određuje projektna pravila, `.kova/skills` razvojne postupke, a `.ai/AI_USAGE.md` evidenciju odluka. Pre pokretanja Kova MCP servera i guardrails provera izgraditi solution u Release konfiguraciji. MCP resursi su `project://instructions` i `project://solution`; alati su `get_project_structure`, `get_git_diff` i `run_unit_tests`. Detaljna priprema i konfiguracija nalaze se u vežbama 6–8 praktikuma.

Evaluacije iz `evals/*.json` sprovodi član tima: izabrati `skill` i `mode`, zadati `input`, proveriti sve stavke `expected.must` i `expected.mustNot`, pa zabeležiti ishod i dokaz u `.ai/AI_USAGE.md`. Unit testovi proveravaju strukturu tih scenarija; ocena odgovora agenta zahteva pregled.

## Usklađivanje sa portalom

U repozitorijumu nastavnog portala pokrenuti `npm run generate:practicum`, preneti generisane Markdown dokumente u odgovarajuće `03 Racunarske vezbe` direktorijume i uskladiti izvore primera. Komanda `npm run check:materials-sync` upoređuje oba repozitorijuma prema sadržaju datoteka. README se održava samo u korenu repozitorijuma.
