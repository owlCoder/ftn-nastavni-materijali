# Vežba 5 — Tok podataka i read modeli

Primer razdvaja zapis od prikaza. Svako prihvaćeno merenje ulazi u zapis redom kojim je stiglo, a prikaz stanice se izvodi iz zapisa i pamti samo poslednje merenje. Merenje koje zakasni ostaje u zapisu, ali ne vraća prikaz unazad; prikaz koji dugo nije osvežen se označava kao zastareo umesto da izgleda kao trenutno stanje.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba05.ConsoleUi
dotnet test Odp.Vezba05.sln
```

## Struktura

```text
src/
  Odp.Vezba05.Domain/          TelemetryReading, StationView, StationViewProjector, FreshnessPolicy
  Odp.Vezba05.Application/     IngestTelemetryHandler, GetStationStatusHandler + portovi
  Odp.Vezba05.Infrastructure/  in-memory zapis merenja, skladište prikaza, satovi
  Odp.Vezba05.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba05.Tests/           testovi projekcije, svežine i use-case-ova
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | `ITelemetryLog` je izvor istine; `StationView` je izveden prikaz |
| Tok | prijem → provera → zapis → projekcija → čitanje |
| Odluka | `StationViewProjector` primenjuje samo novije merenje; `FreshnessPolicy` označava zastarelost |
| Failure scenario | merenje #2 stiže posle merenja #3; stanica prestaje da šalje |
| Test | zakasnelo merenje ostaje u zapisu, a prikaz se ne menja; star prikaz je označen |
| Dokaz | `IngestOutcome.ViewChanged` i `StationStatus.IsStale` sa izmerenom starošću |

## SOLID i Clean Architecture

- Upis i čitanje su odvojeni use-case-ovi sa odvojenim modelima (CQRS u malom).
- `StationViewProjector` i `FreshnessPolicy` su domenski servisi bez zavisnosti od skladišta i vremena.
- Zapis i skladište prikaza su portovi, pa se read model može ponovo izgraditi iz zapisa.
- Zastarelost je eksplicitan podatak u odgovoru, a ne pretpostavka klijenta.

## Zadatak

Dodati use-case `RebuildStationView` koji briše prikaz stanice i ponovo ga izvodi iz zapisa. Testom pokazati da je rezultat isti bez obzira na redosled kojim su merenja prvobitno stigla.
