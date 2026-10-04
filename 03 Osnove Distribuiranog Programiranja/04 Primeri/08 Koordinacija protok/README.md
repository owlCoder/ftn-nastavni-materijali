# Vežba 8 — Koordinacija, protok i eventualna konzistentnost

Primer pokazuje dve garancije koje sistem mora da izabere kada raste. Prva je vlasništvo: samo jedan worker sme da menja raspored stanice, vlasništvo je vremenski ograničeno (lease), a resurs odbija upis bivšeg vlasnika po zastarelom tokenu. Druga je protok: kada je red pun, pošiljalac dobija jasan odgovor umesto da posao neograničeno čeka.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba08.ConsoleUi
dotnet test Odp.Vezba08.sln
```

## Struktura

```text
src/
  Odp.Vezba08.Domain/          Lease, LeasePolicy, FencingPolicy, IntakePolicy
  Odp.Vezba08.Application/     AcquireLeaseHandler, WriteScheduleHandler, SubmitJobHandler + portovi
  Odp.Vezba08.Infrastructure/  in-memory lease, raspored sa tokenom, red poslova, satovi
  Odp.Vezba08.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba08.Tests/           testovi politika i oba toka
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | `Lease` — jedan vlasnik resursa u jednom trenutku, sa rokom |
| Identitet operacije | `FencingToken` — raste pri svakoj promeni vlasnika |
| Odluka | `LeasePolicy`, `FencingPolicy` i `IntakePolicy` daju ishod sa stabilnim kodom |
| Failure scenario | vlasnik zastane, lease istekne, a on se probudi i pokuša upis; red je pun |
| Test | upis sa starim tokenom je odbijen; treći posao dobija `queue_overloaded` |
| Dokaz | važeći raspored posle preuzimanja i kod odgovora za svaki posao |

## SOLID i Clean Architecture

- Tri pravila su tri domenska servisa; nijedan ne zna za skladište ni za vreme.
- Zaštita od bivšeg vlasnika je na strani resursa (`IScheduleStore.HighestToken`), jer bivši vlasnik ne zna da je smenjen.
- Vreme je port (`IClock`), pa se istek lease-a testira pomeranjem sata.
- Preopterećenje je eksplicitan ishod (`queue_overloaded`), ne izuzetak i ne tiho čekanje.

## Zadatak

Dodati u prikaz rasporeda podatak o svežini: vreme poslednjeg upisa i oznaku `IsStale` kada je prikaz stariji od zadatog praga. Testom pokazati da čitalac vidi zastareo, ali jasno označen podatak dok vlasnik ne upiše novi.
