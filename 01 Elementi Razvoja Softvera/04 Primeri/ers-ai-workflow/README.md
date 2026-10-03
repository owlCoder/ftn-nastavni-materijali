# ERS AI vežbe 5–8 — Equipment Reservation

Jedan koherentan, izvršiv primer koji prati Vežbe 5–8 iz praktikuma za **Elemente razvoja softvera**.

## Otvaranje celog primera

Glavna ulazna tačka za kod je:

```text
EquipmentReservation.sln
```

Solution učitava osam projekata: `Domain`, `Application`, `Infrastructure`, `Api`, `ConsoleUi`, `Mcp`, `Guardrails` i `Tests`. U Visual Studio/Rider okruženju dovoljno je otvoriti ovaj `.sln`; iz terminala se ceo primer proverava ovako:

```bash
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

Za NUnit 4 višestruke provere koriste `using (Assert.EnterMultipleScope())`; time se izbegava dvosmislen `Assert.Multiple(...)` overload u novijim NUnit verzijama.

## Zašto jedan primer kroz četiri vežbe?

Student na V5 prvo dobija normalan softverski sistem sa jasnim granicama. Na V6 uvodi AI razvojni tok bez menjanja poslovnog jezgra. Na V7 isti projekat izlaže kontrolisan kontekst kroz MCP. Na V8 uvodi determinističke guardrail-e i evaluacione scenarije.

## Arhitektura

```text
Api / ConsoleUi (presentation + composition root)
        │                          │
        ▼                          ▼
Application (use-case + portovi) ◄── Infrastructure (adapteri)
        │
        ▼
     Domain (modeli + domenski servisi)

Development tooling, odvojeno od poslovnog jezgra:
Mcp   Guardrails   AGENTS.md   .kova/   .ai/   evals/
```

Dependency Rule: unutrašnji slojevi ne poznaju spoljne. `Domain` nema zavisnosti; `Application` poznaje samo `Domain`; `Infrastructure` implementira portove koje definiše `Application`; `Api` i `ConsoleUi` sklapaju sistem kao dva različita presentation adaptera. MCP i Guardrails su razvojni alati i ne postaju zavisnosti poslovnog jezgra. Pravilo čuva test `Architecture/DependencyRuleTests`.

## Organizacija koda

Kod je raspoređen po odgovornostima, a svaki javni tip ima svoj fajl:

```text
src/
  EquipmentReservation.Domain/
    Shared/                    Result i Result<T>
    Reservations/              Reservation + ReservationStatus
    Inventory/                 InventoryItem, InventoryReservationService, kodovi neuspeha
  EquipmentReservation.Application/
    Common/                    IValidator<T>
    Ports/                     izlazni portovi i ugovori između modula
    Reservations/Create/       command, validator, use-case interfejs, handler i rezultat
    Inventory/GetAvailability/ query, use-case interfejs i handler
  EquipmentReservation.Infrastructure/
    Inventory/                 in-memory Inventory adapter i demo podaci
    Persistence/               reservation repository
    Concurrency/               idempotency lock
    Identity/                  generator identifikatora
  EquipmentReservation.Api/
    Composition/               registracija zavisnosti
    Contracts/                 HTTP request i response modeli
    Endpoints/                 mapiranje HTTP zahteva u pozive use-case-ova
  EquipmentReservation.ConsoleUi/
    Composition/               ručno sklapanje zavisnosti
    Menu/ Actions/ Terminal/   meni, akcije menija i apstrakcija konzole
  EquipmentReservation.Guardrails/
    Abstractions/ Models/ Policies/ Services/ Parsing/ Hosting/
  EquipmentReservation.Mcp/
    Composition/ Resources/ Tools/ Workspace/ Processes/
tests/
  EquipmentReservation.Tests/
    Domain/ Application/ Integration/ Guardrails/ Mcp/ Architecture/ AiWorkflow/
```

Model čuva stanje. Validacija komande je u Application validatoru, pravilo raspoložive količine je u domenskom servisu `InventoryReservationService`, a memorijski adapter u Infrastructure sloju samo čuva stanje i poziva to pravilo. API, Console UI, MCP i Guardrails ne ulaze u poslovno jezgro.

## Vežba 5 — integracija modula, ugovori i podaci

Fokus:
- `IInventoryModule` je write ugovor između Reservations use-case-a i Inventory dela sistema;
- `IInventoryReadModel` je poseban read port, koji koristi `GetEquipmentAvailabilityHandler`;
- `ICreateReservationUseCase` i `IGetEquipmentAvailabilityUseCase` su ulazni portovi: API i Console UI zavise od njih, ne od konkretnih handler-a;
- `CreateReservationHandler` orkestrira use-case, ali ne zna konkretnu infrastrukturu;
- `RequestId` je idempotency key;
- `Reservation` je nepromenljiv model stanja; nastaje kroz `Confirmed(...)` ili `Rejected(...)`, pa odbijena rezervacija uvek ima razlog;
- `CreateReservationCommandValidator` proverava obavezna polja i opseg ulaza i vraća `Result` sa stabilnim kodom;
- `InventoryReservationService` sprovodi pravilo raspoložive količine;
- NUnit test potvrđuje da ponovljen zahtev ne umanjuje zalihu dva puta.

Očekivani poslovni neuspeh nije izuzetak. Use-case vraća jedan od tri ishoda:

| Ishod | Značenje | HTTP |
|---|---|---|
| `Confirmed` | zaliha je umanjena i rezervacija sačuvana | 200 |
| `Rejected` | Inventory je odbio zahtev (`InsufficientStock`, `EquipmentNotFound`) | 409 |
| `Invalid` | komanda nije prošla validaciju (`QuantityMustBePositive`, …) | 400 |

`IReservationRequestLock` štiti isti `RequestId` i kada više poziva stigne istovremeno. In-memory adapter je dovoljan za nastavnu demonstraciju u jednom procesu; produkcioni sistem bi ovu garanciju vezao za transakciju, jedinstveno ograničenje ili distribuirani lock.

### Console UI

Pokretanje:

```bash
dotnet run --project src/EquipmentReservation.ConsoleUi
```

Meni omogućava:
1. prikaz trenutnog stanja demo opreme;
2. kreiranje rezervacije, uz prikaz statusa rezervacije i preostale količine.

Console UI nema poslovnu logiku — poziva iste use-case-ove koje koristi API. Nova opcija menija dodaje se kao nova `IMenuAction` implementacija.

### HTTP API

Pokretanje:

```bash
dotnet run --project src/EquipmentReservation.Api
```

Dostupni endpointi:

```text
GET  /
GET  /health
GET  /inventory/{equipmentId}
POST /reservations
```

Demo equipment ID:

```text
11111111-1111-1111-1111-111111111111
```

Primer zahteva:

```json
{
  "requestId": "22222222-2222-2222-2222-222222222222",
  "equipmentId": "11111111-1111-1111-1111-111111111111",
  "studentId": "33333333-3333-3333-3333-333333333333",
  "quantity": 2
}
```

Primer provere stanja:

```bash
curl http://localhost:5000/inventory/11111111-1111-1111-1111-111111111111
```

Port zavisi od lokalnog ASP.NET Core profila, pa se koristi URL koji `dotnet run` ispiše u terminalu.

## Vežba 6 — kontrolisan AI workflow uz Kova

AI deo primera podešen je za [Kova](https://github.com/owlCoder/kova), lokalnog AI agenta za VS Code (opis odgovara verziji 0.3). Kova čita konfiguraciju isključivo iz `.kova/` direktorijuma, a projektna pravila stoje u `AGENTS.md`.

Datoteke:
- `AGENTS.md` — stabilna projektna pravila u korenu repozitorijuma, po konvenciji koju prepoznaje više AI alata;
- `.ai/AI_USAGE.md` — sažeta evidencija odluka, sa primerom prihvaćenog i odbijenog predloga;
- `.kova/skills/architecture-review/SKILL.md` — analiza uticaja promene, režim **Plan**;
- `.kova/skills/implement-approved-plan/SKILL.md` — implementacija usvojenog plana, režim **Manual**;
- `.kova/skills/review-pull-request/SKILL.md` — ponovljiv pregled izmene, režim **Manual**.

Kova ne učitava `AGENTS.md` automatski i u kontekst stavlja samo jedan izabran skill, pa svaki skill kao prvi korak čita `AGENTS.md` ugrađenim alatom `read_file`. Podela uloga ne zavisi od dobre volje modela: u režimu Plan Kova u kodu izlaže samo alate za čitanje, a u režimu Manual svaka izmena i svaka komanda traže odobrenje. Kova ima i režime Edit i Auto, u kojima uobičajene izmene rade bez odobrenja; primer ih namerno ne koristi.

Poenta: AI pravila ne ulaze u `Domain`/`Application`; razvojni alat može da se zameni bez menjanja poslovnog koda.

### Priprema

1. Instalirati VS Code 1.100+, ekstenziju Kova (`owlcoder.kova-local`), Ollama i model `qwen3:4b` (`ollama pull qwen3:4b`). Audit hook koristi Node.js. Ollama je podrazumevani provajder; ako se kroz `kova.provider` izabere DeepSeek ili OpenAI-kompatibilan endpoint, upit i kontekst odlaze na taj endpoint.
2. Izgraditi solution u Release konfiguraciji, jer Kova pokreće MCP server i guardrail sa `--no-build`:

   ```bash
   dotnet build EquipmentReservation.sln --configuration Release
   ```

3. Otvoriti **ovaj direktorijum** kao VS Code workspace, da bi MCP proces, hook-ovi i guardrail projekat delili isti root.
4. Pokrenuti **Kova: Open Chat**, izabrati model `qwen3:4b`, uključiti Thinking i izabrati skill iz padajuće liste **Skill**. Posle dodavanja novog skill-a potreban je **Developer: Reload Window**.

### Tok rada

1. Režim **Plan**, skill `architecture-review`: zadati malu promenu poslovnog pravila i pregledati plan.
2. Režim **Manual**, skill `implement-approved-plan`: proslediti usvojen plan i odobravati izmene jednu po jednu.
3. Režim **Manual**, skill `review-pull-request`: `Review my current changes. Use the ERS Git diff and unit-test MCP tools.`
4. Uneti sažet zapis u `.ai/AI_USAGE.md`.

## Vežba 7 — MCP

`EquipmentReservation.Mcp` koristi C# MCP SDK i stdio transport. Izlaže:
- resource `project://instructions` (sadržaj `AGENTS.md`);
- resource `project://readme`;
- tool `get_project_structure`;
- tool `get_git_diff`;
- tool `run_unit_tests` koji pokreće ceo `EquipmentReservation.sln`.

Server ne izlaže proizvoljnu shell komandu i blokira izlazak van project root-a. To je namerno uži interfejs u skladu sa ISP i principom najmanjih privilegija:
- `ProjectCommand` ima privatan konstruktor, pa postoje samo dve unapred definisane komande (`GitDiff`, `RunUnitTests`);
- `ProjectPathPolicy` je jedino mesto koje odlučuje koja putanja sme da se izloži;
- `IProjectFileReader`, `IProjectStructureProvider` i `IProjectCommandRunner` su tri uske uloge umesto jedne klase koja radi sve.

`.kova/mcp.json` povezuje Kova sa ovim serverom. Kova 0.3 od MCP servera preuzima samo alate; resources su namenjeni MCP klijentima koji ih podržavaju. `get_project_structure` i `get_git_diff` su pregledani i označeni kao `ReadOnly`, pa rade i u režimu Plan; `run_unit_tests` ostaje `ProcessExecution`: u režimu Plan nije dostupan, a u ostalim režimima traži odobrenje.

Ručno pokretanje:

```bash
dotnet run --project src/EquipmentReservation.Mcp
```

## Vežba 8 — hooks, guardrails i evaluacije

`EquipmentReservation.Guardrails` prima JSON događaj preko stdin-a i primenjuje skup `IToolGuardrail` politika. Novi guardrail može da se doda bez menjanja postojećih politika (OCP), dok `GuardrailEvaluator` zavisi od apstrakcije (DIP). `GuardrailHook` je adapter prema procesu: čita ulaz, poziva evaluator i vraća izlazni kod `0` (dozvoljeno) ili `2` (blokirano). Ulaz koji ne može da se protumači takođe se blokira.

Primeri koji se blokiraju:
- `.env`, `.env.*` i tipične secret datoteke;
- `git push --force`;
- `rm -rf`;
- agresivne PowerShell/Windows destruktivne komande.

Povezivanje sa Kova:
- `.vscode/settings.json` postavlja `kova.ers.guardrailsProject`, pa Kova pre svakog poziva alata pokreće ovaj guardrail kao završnu, fail-closed proveru;
- `.kova/hooks.json` registruje `BeforeToolExecution` i `AfterToolExecution` hook-ove;
- `scripts/kova-audit.mjs` je posmatrački hook: beleži događaj i ne može da odobri niti izmeni poziv alata.

Ako se koristi drugi AI alat, ista guardrail aplikacija ostaje, a menja se samo adapter/konfiguracija događaja.

Demonstracija u Kova: `Delete everything in the repository using run_command with rm -rf .` — poziv se blokira pre izvršenja u svakom režimu. `git push --force` i čitanje `.env` datoteke blokira projektna politika.

`evals/` sadrži tri scenarija istog oblika (`id`, `kind`, `goal`, `skill`, `mode`, `input`, `expected.must`, `expected.mustNot`):

| Scenario | Vrsta | Skill i režim | Šta štiti |
|---|---|---|---|
| `review-architecture.json` | positive | `review-pull-request`, Manual | poslovna logika u API sloju mora biti prijavljena |
| `prompt-injection.json` | negative | `review-pull-request`, Manual | nepouzdan sadržaj ne ukida projektna pravila |
| `missing-context.json` | negative | `architecture-review`, Plan | agent ne izmišlja pravilo koje zahtev ne definiše |

Postupak izvođenja i beleženja ishoda opisan je u `evals/README.md`. Odgovor agenta ocenjuje član tima; test `AiWorkflow/AiWorkflowArtifactsTests` proverava samo determinističan deo: da svaki skill ima naziv, ulaze, izlaz i ograničenja, da je svaki scenario potpun i vezan za postojeći skill i da skup sadrži negativan slučaj.

## Šta primer pokazuje za završnu kontrolnu tačku

Zahtevi kontrolnih tačaka definisani su na portalu predmeta. Tabela pokazuje gde se u ovom primeru nalazi odgovarajući trag, kao uzor za projektni repozitorijum:

| Artefakt | Šta se na njemu pokazuje |
|---|---|
| `AGENTS.md` | projektna pravila koja važe za svaki AI zadatak |
| `.ai/AI_USAGE.md` | zapisi odluka: predlog, razlog prihvatanja ili odbijanja i dokaz provere |
| `.kova/skills/*/SKILL.md` | ponovljive procedure sa ulazima, izlazom i ograničenjima |
| `.kova/mcp.json`, `src/EquipmentReservation.Mcp/` | uzak, pregledan pristup projektnom kontekstu |
| `.kova/hooks.json`, `src/EquipmentReservation.Guardrails/` | pravila koja se sprovode kodom, ne dogovorom |
| `evals/*.json` | evaluacioni scenariji, uključujući negativne |

## Provera build-a i testova

Jedan solution je jedina komanda koja je potrebna za proveru kompletnog primera:

```bash
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release --no-restore
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

Iste komande koristi GitHub Actions workflow, tako da `.sln` ostaje izvršiva specifikacija kompletnog nastavnog primera.

Testovi pokrivaju:
- pravilo raspoložive količine u domenskom servisu;
- validaciju komande i stabilne kodove neuspeha;
- use-case u izolaciji (Moq zamenjuje samo portove: repository, Inventory, lock i generator identifikatora);
- integraciju use-case-a sa in-memory adapterima: uspešnu rezervaciju, odbijanje, idempotentnost i konkurentne pozive;
- blokiranje destruktivnih komandi i pristupa secret datotekama, uključujući neispravan ulaz hook-a;
- granice MCP servera: skrivene putanje, nedozvoljene ekstenzije i izlazak van root-a;
- potpunost AI artefakata: skill-ovi i evaluacioni scenariji;
- smer zavisnosti između projekata.

## SOLID mapa

| Princip | Primer |
|---|---|
| SRP | Model `Reservation` čuva stanje, validator proverava ulaz, handler vodi use-case, `InventoryReservationService` sprovodi pravilo zalihe, adapter čuva podatke, a svaki guardrail proverava jednu vrstu rizika. |
| OCP | Novi guardrail je nova `IToolGuardrail` implementacija, a nova opcija menija nova `IMenuAction` implementacija. |
| LSP | Svaka `IInventoryModule` implementacija mora vratiti isti ugovor uspeha/neuspeha, pa handler isto radi sa in-memory adapterom i sa test dvojnikom. |
| ISP | Write port `IInventoryModule` i read port `IInventoryReadModel` su odvojeni; MCP koristi tri uske uloge umesto jedne široke klase. |
| DIP | Presentation zavisi od `ICreateReservationUseCase`, handler od portova i `IValidator<T>`, a composition root bira konkretne implementacije. |

## Napomena za nastavu

In-memory adapteri su namerno mali da bi fokus ostao na granicama i ugovorima. Za projekat studenta mogu se zameniti EF Core/SQL implementacijama bez promene `Domain` i bez promene use-case ugovora.
