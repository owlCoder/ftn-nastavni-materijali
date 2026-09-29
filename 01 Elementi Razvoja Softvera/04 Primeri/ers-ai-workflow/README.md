# ERS AI vežbe 5–8 — Equipment Reservation

Jedan koherentan, izvršiv primer koji prati Vežbe 5–8 iz praktikuma za **Elemente razvoja softvera**.

## Otvaranje celog primera

Glavna ulazna tačka za kod je:

```text
EquipmentReservation.sln
```

Solution učitava osam projekata: `Domain`, `Application`, `Infrastructure`, `Api`, `ConsoleUi`, `Mcp`, `Guardrails` i `Tests`. U Visual Studio/Rider okruženju dovoljno je otvoriti ovaj `.sln`; iz terminala se ceo primer proverava ovako:

```bash
cd examples/ers-ai-workflow
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

Za NUnit 4 višestruke provere koriste `using (Assert.EnterMultipleScope())`; time se izbegava dvosmislen `Assert.Multiple(...)` overload u novijim NUnit verzijama.

## Zašto jedan primer kroz četiri vežbe?

Student na V5 prvo dobija normalan softverski sistem sa jasnim granicama. Na V6 uvodi AI razvojni tok bez menjanja poslovnog jezgra. Na V7 isti projekat izlaže kontrolisan kontekst kroz MCP. Na V8 uvodi determinističke guardrail-e i evaluacione scenarije.

## Arhitektura

```text
Domain
  ↑
Application (use-case + portovi)
  ↑                 ↑
Infrastructure      Api / ConsoleUi (presentation + composition root)

Development tooling, odvojeno od poslovnog jezgra:
Mcp   Guardrails   .ai/   evals/
```

Dependency Rule: unutrašnji slojevi ne poznaju spoljne. `Domain` nema zavisnosti; `Application` poznaje samo `Domain`; `Infrastructure` implementira portove koje definiše `Application`; `Api` i `ConsoleUi` sklapaju sistem kao dva različita presentation adaptera. MCP i Guardrails su razvojni alati i ne postaju zavisnosti poslovnog jezgra.

## Organizacija koda

Kod je raspoređen po odgovornostima, a svaki javni tip ima svoj fajl:

```text
src/
  EquipmentReservation.Domain/
    Reservations/              Reservation + ReservationStatus
  EquipmentReservation.Application/
    Ports/                     interfejsi i DTO ugovori
    Reservations/Create/       command, validator, handler i rezultat
  EquipmentReservation.Infrastructure/
    Inventory/                 in-memory Inventory adapter
    Persistence/               reservation repository
    Concurrency/               idempotency lock
  EquipmentReservation.Api/
    Contracts/                 HTTP request modeli
  EquipmentReservation.Guardrails/
    Abstractions/ Models/ Policies/ Services/ Parsing/
  EquipmentReservation.Mcp/
    Resources/ Tools/ Workspace/
tests/
  EquipmentReservation.Tests/
    Application/ Guardrails/
```

Domain model čuva stanje. Validacija komande je u Application validatoru, pravilo raspoložive količine je iza `IInventoryModule` ugovora, a konkretna memorijska implementacija je u Infrastructure sloju. API, Console UI, MCP i Guardrails ne ulaze u poslovno jezgro.

## Vežba 5 — integracija modula, ugovori i podaci

Fokus:
- `IInventoryModule` je write ugovor između Reservations use-case-a i Inventory dela sistema;
- `IInventoryReadModel` je poseban read port za UI/API adaptere;
- `CreateReservationHandler` orkestrira use-case, ali ne zna konkretnu infrastrukturu;
- `RequestId` je idempotency key;
- `Reservation` je nepromenljiv model stanja bez poslovnih metoda;
- `CreateReservationCommandValidator` proverava obavezna polja i opseg ulaza;
- `CreateReservationHandler` vodi use-case, dok `IInventoryModule` čuva pravilo raspoložive količine iza jasnog ugovora;
- NUnit test potvrđuje da ponovljen zahtev ne umanjuje zalihu dva puta.

`IReservationRequestLock` štiti isti `RequestId` i kada više poziva stigne istovremeno. In-memory adapter je dovoljan za nastavnu demonstraciju u jednom procesu; produkcioni sistem bi ovu garanciju vezao za transakciju, jedinstveno ograničenje ili distribuirani lock.

### Console UI

Pokretanje:

```bash
dotnet run --project src/EquipmentReservation.ConsoleUi
```

Meni omogućava:
1. prikaz trenutnog stanja demo opreme;
2. kreiranje rezervacije;
3. prikaz statusa rezervacije i preostale količine.

Console UI nema poslovnu logiku — poziva isti `CreateReservationHandler` i Application portove koje koristi ostatak sistema.

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

## Vežba 6 — kontrolisan AI workflow

Datoteke:
- `.ai/AI_INSTRUCTIONS.md` — stabilna projektna pravila;
- `.ai/AI_USAGE.md` — sažeta evidencija odluka;
- `.ai/skills/review-pull-request/SKILL.md` — ponovljiva procedura;
- `.ai/agents/architecture-reviewer.md` — read-only uloga;
- `.ai/agents/implementer.md` — implementaciona uloga.

Poenta: AI pravila ne ulaze u `Domain`/`Application`; razvojni alat može da se zameni bez menjanja poslovnog koda.

## Vežba 7 — MCP

`EquipmentReservation.Mcp` koristi C# MCP SDK i stdio transport. Izlaže:
- resource `project://instructions`;
- resource `project://readme`;
- tool `get_project_structure`;
- tool `get_git_diff`;
- tool `run_unit_tests` koji pokreće ceo `EquipmentReservation.sln`.

Server ne izlaže proizvoljnu shell komandu i blokira izlazak van project root-a. To je namerno uži interfejs u skladu sa ISP i principom najmanjih privilegija.

Pokretanje:

```bash
dotnet run --project src/EquipmentReservation.Mcp
```

## Vežba 8 — hooks, guardrails i evaluacije

`EquipmentReservation.Guardrails` prima JSON događaj preko stdin-a i primenjuje skup `IToolGuardrail` politika. Novi guardrail može da se doda bez menjanja postojećih politika (OCP), dok `GuardrailEvaluator` zavisi od apstrakcije (DIP).

Primeri koji se blokiraju:
- `.env` i tipične secret datoteke;
- `git push --force`;
- `rm -rf`;
- agresivne PowerShell/Windows destruktivne komande.

`.claude/settings.json` pokazuje kako guardrail može da se veže za `PreToolUse` hook. Ako se koristi drugi AI alat, ista guardrail aplikacija ostaje, menja se samo adapter/konfiguracija događaja.

`evals/` sadrži tri scenarija:
1. arhitektonska regresija;
2. prompt injection;
3. nedovoljan kontekst.

## Provera build-a i testova

Jedan solution je jedina komanda koja je potrebna za proveru kompletnog primera:

```bash
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release --no-restore
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

Iste komande koristi GitHub Actions workflow, tako da `.sln` ostaje izvršiva specifikacija kompletnog nastavnog primera.

Testovi pokrivaju:
- uspešnu rezervaciju i umanjenje raspoložive količine;
- odbijanje kada nema dovoljno opreme ili oprema ne postoji;
- validaciju neispravnog zahteva i idempotentnost use-case-a, uključujući konkurentne pozive;
- blokiranje destruktivnih komandi;
- blokiranje pristupa `.env` datoteci.

## SOLID mapa

| Princip | Primer |
|---|---|
| SRP | Model `Reservation` čuva stanje, validator proverava ulaz, handler vodi use-case, inventory adapter sprovodi svoj ugovor, a svaki guardrail proverava jednu vrstu rizika. |
| OCP | Novi guardrail se dodaje kao nova `IToolGuardrail` implementacija. |
| LSP | Svaka `IInventoryModule` implementacija mora vratiti isti ugovor uspeha/neuspeha. |
| ISP | Write port `IInventoryModule` i read port `IInventoryReadModel` su odvojeni; adapter dobija samo operacije koje su mu potrebne. |
| DIP | `CreateReservationHandler`, API i Console UI koriste apstrakcije iz Application sloja umesto da poslovna pravila vezuju za infrastrukturu. |

## Napomena za nastavu

In-memory adapteri su namerno mali da bi fokus ostao na granicama i ugovorima. Za projekat studenta mogu se zameniti EF Core/SQL implementacijama bez promene `Domain` i bez promene use-case ugovora.
