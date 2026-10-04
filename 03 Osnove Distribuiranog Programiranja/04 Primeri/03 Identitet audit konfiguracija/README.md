# Vežba 3 — Identitet, audit, observability i konfiguracija

Primer prati jednu operaciju kroz dve komponente. Zakazivanje kontakta sa stanicom dobija identifikator operacije (`CorrelationId`) i identitet aktera; isti identifikator stiže do stanice i do revizijskog traga, pa se tok može rekonstruisati i kada delovi sistema vode odvojene logove. Ograničenje trajanja kontakta dolazi iz konfiguracije koja se proverava pre pokretanja.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba03.ConsoleUi
dotnet test Odp.Vezba03.sln
```

## Struktura

```text
src/
  Odp.Vezba03.Domain/          OperationContext, ContactRequest, ContactRequestValidator, ContactLimitsValidator
  Odp.Vezba03.Application/     ScheduleContactHandler + portovi (stanica, revizijski trag, sat)
  Odp.Vezba03.Infrastructure/  in-memory stanica koja beleži pozive, revizijski trag, sistemski sat
  Odp.Vezba03.ConsoleUi/       composition root sa proverom konfiguracije i demonstracija
tests/
  Odp.Vezba03.Tests/           testovi pravila, konfiguracije i use-case-a
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | centar je vlasnik zahteva; stanica je vlasnik svog rasporeda |
| Identitet operacije | `OperationContext` — `CorrelationId` i `ActorId` |
| Odluka | `ContactRequestValidator` i odgovor stanice daju ishod sa stabilnim kodom |
| Failure scenario | stanica odbija rezervaciju; zahtev prelazi ograničenje iz konfiguracije |
| Test | isti `CorrelationId` u pozivu stanice i u revizijskom zapisu |
| Dokaz | `AuditEntry` za svaki ishod, uključujući odbijanje pre poziva stanice |

## SOLID i Clean Architecture

- Pravilo o trajanju je u `Domain` sloju; vrednost ograničenja je konfiguracija koju composition root prosleđuje.
- Upis i čitanje revizijskog traga su odvojeni interfejsi (`IAuditLog`, `IAuditTrail`) — use-case dobija samo upis (ISP).
- `ScheduleContactHandler` zavisi od porta `IStationGateway` (DIP), pa se stanica menja bez izmene pravila.
- Neispravna konfiguracija zaustavlja pokretanje sa kodom greške, umesto da tiho promeni ponašanje.

## Zadatak

Dodati u `OperationContext` identifikator pozivajuće komponente (`Source`) i upisati ga u revizijski zapis. Proširiti test tako da dokazuje da zapis sadrži i izvor operacije.
