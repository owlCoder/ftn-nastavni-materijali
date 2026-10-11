# Equipment Reservation — ERS vežbe 5–8

Rezervacija opreme kroz isti poslovni model, Console UI i HTTP API. Primer povezuje integraciju modula sa AI razvojnim alatima, MCP-om, guardrails proverama i evaluacijama.

| Vežba | Tema | Deo primera |
|---|---|---|
| 5 | Integracija modula i ugovori | `Application`, `Infrastructure`, `Api`, `ConsoleUi` |
| 6 | Kontrolisan razvoj uz AI | [Projektna pravila](<AGENTS.md>), [.kova/](<.kova/>), [evidencija odluka](<.ai/AI_USAGE.md>) |
| 7 | MCP | [EquipmentReservation.Mcp](<src/EquipmentReservation.Mcp/>) |
| 8 | Hooks, guardrails i evaluacije | [Guardrails](<src/EquipmentReservation.Guardrails/>), [evals](<evals/README.md>) |

## Priprema i provera

Potreban je .NET 8 SDK. Otvoriti [EquipmentReservation.sln](<EquipmentReservation.sln>) u razvojnom okruženju ili izvršiti iz ovog direktorijuma:

```bash
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release --no-restore
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

Testovi pokrivaju poslovna pravila, validaciju, idempotentnost, konkurentne zahteve, granice arhitekture i ponašanje razvojnih alata.

## Arhitektura

`Domain` sadrži stanje i domenska pravila, bez projektnih zavisnosti. `Application` zavisi samo od `Domain` i definiše use-case-ove i portove. `Infrastructure` implementira portove; `Api` i `ConsoleUi` mapiraju ulaz i sklapaju zavisnosti. `Mcp` i `Guardrails` služe razvoju i ne ulaze u poslovno jezgro.

`InventoryReservationService` proverava raspoloživu količinu. `CreateReservationHandler` orkestrira rezervaciju preko write porta `IInventoryModule`; čitanje koristi `IInventoryReadModel`. `RequestId` je idempotency key, a `IReservationRequestLock` štiti istovremene pozive za isti zahtev.

## Pokretanje aplikacije

```bash
dotnet run --project src/EquipmentReservation.ConsoleUi
dotnet run --project src/EquipmentReservation.Api
```

Pokrenuti željeni adapter. Za API koristiti URL ispisan u terminalu. Dostupni su `GET /`, `GET /health`, `GET /inventory/{equipmentId}` i `POST /reservations`.

Primer tela zahteva za `POST /reservations`:

```json
{
  "requestId": "22222222-2222-2222-2222-222222222222",
  "equipmentId": "11111111-1111-1111-1111-111111111111",
  "studentId": "33333333-3333-3333-3333-333333333333",
  "quantity": 2
}
```

| Ishod | Značenje | HTTP |
|---|---|---|
| `Confirmed` | Rezervacija je sačuvana, zaliha umanjena | 200 |
| `Rejected` | Inventory odbija zahtev sa razlogom | 409 |
| `Invalid` | Komanda ne prolazi validaciju | 400 |

Adapteri čuvaju podatke u memoriji. Zaštita konkurentnih zahteva važi u jednom procesu; trajno skladištenje i koordinacija više procesa zahtevaju odgovarajući adapter.

## AI razvojni tok

Konfiguracija primera pripremljena je za Kova 0.3.2, VS Code 1.100+ i Ollama model `qwen3:4b`. Audit hook koristi Node.js.

1. Instalirati Kova ekstenziju `owlcoder.kova-local` i preuzeti model: `ollama pull qwen3:4b`.
2. Izgraditi solution u Release konfiguraciji. MCP i guardrail konfiguracija koriste `--no-build`.
3. Otvoriti ovaj direktorijum kao VS Code workspace. U **Kova: Open Chat** izabrati model, Thinking i skill; posle dodavanja skill-a pokrenuti **Developer: Reload Window**.
4. Koristiti `architecture-review` u režimu **Plan**, zatim `implement-approved-plan` i `review-pull-request` u režimu **Manual**.
5. Pregledati izmene, pokrenuti testove i zabeležiti prihvaćene ili odbijene predloge u [.ai/AI_USAGE.md](<.ai/AI_USAGE.md>).

`AGENTS.md` daje zajednička projektna pravila; skill-ovi su u [.kova/skills/](<.kova/skills/>). Plan služi analizi, a Manual traži odobrenje za izmene i komande. Ollama je podrazumevani provajder; promena `kova.provider` usmerava upit i kontekst na izabrani endpoint.

## MCP i guardrails

[.kova/mcp.json](<.kova/mcp.json>) povezuje stdio MCP server. Alati su `get_project_structure`, `get_git_diff` i `run_unit_tests`; resources `project://instructions` i `project://readme` namenjeni su klijentima koji ih podržavaju. Server ograničava putanje na projektni direktorijum i izvršava unapred definisane komande. Test alat pokreće proces i zahteva odgovarajuće odobrenje u Kova; nije dostupan u režimu Plan.

Ručno pokretanje servera: `dotnet run --project src/EquipmentReservation.Mcp`.

`Guardrails` obrađuje JSON događaj sa stdin-a: kod `0` dozvoljava poziv, a `2` ga blokira. Neispravan ulaz, pristup secret datotekama i destruktivne komande se blokiraju. Povezivanje je u [.vscode/settings.json](<.vscode/settings.json>) i [.kova/hooks.json](<.kova/hooks.json>); [audit hook](<scripts/kova-audit.mjs>) beleži događaje, bez odobravanja poziva.

[Evaluacioni scenariji](<evals/README.md>) proveravaju ponašanje agenta. Ocenu donosi član tima; `AiWorkflowArtifactsTests` proverava strukturu artefakata. Obim studentskog projekta i kontrolne tačke određuju dokumenti predmeta.

[Pregled primera](<../README.md>) · [Materijali predmeta](<../../README.md>)
