# Vežba 2 — Ugovori, simulatori i ponovljivost

Primer pokazuje dve stvari koje omogućavaju da se komponente razvijaju nezavisno: poruka ima eksplicitan ugovor koji se proverava na ulazu, a udaljena stanica se zamenjuje simulatorom čiji je tok u potpunosti određen scenarijom. Isti scenario uvek daje iste poruke, pa se i greška može ponoviti.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba02.ConsoleUi
dotnet test Odp.Vezba02.sln
```

## Struktura

```text
src/
  Odp.Vezba02.Domain/          TelemetryMessage, TelemetryContract, TelemetryContractValidator, Result
  Odp.Vezba02.Application/     IngestSimulatedTelemetryHandler + port IStationSimulator
  Odp.Vezba02.Infrastructure/  ScriptedStationSimulator i SimulationScenario
  Odp.Vezba02.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba02.Tests/           testovi ugovora, simulatora i use-case-a
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | stanica je vlasnik merenja; centar je vlasnik odluke o prijemu |
| Ugovor | `TelemetryMessage` verzije `1.0` sa dozvoljenim opsegom signala |
| Odluka | `TelemetryContractValidator` — poruka se prihvata ili odbija sa stabilnim kodom |
| Failure scenario | svaka N-ta poruka simulatora krši ugovor (`signal_out_of_range`) |
| Test | isti scenario daje iste poruke; neispravne poruke su odbijene po rednom broju |
| Dokaz | `IngestionReport` sa brojem prihvaćenih i listom odbijenih poruka |

## SOLID i Clean Architecture

- Ugovor i njegova provera su u `Domain` sloju; simulator je adapter u `Infrastructure`.
- Use-case zavisi od porta `IStationSimulator` (DIP), pa se simulator može zameniti pravom stanicom bez izmene pravila.
- Slučajnost je kontrolisan ulaz: `Seed` je deo scenarija, ne okoline.
- Očekivana greška ulaza je `Result` sa kodom, ne izuzetak.

## Zadatak

Dodati u scenario kašnjenje poruke (`DelayEvery`), tako da svaka N-ta poruka nosi `MeasuredAt` stariji od prethodne. Uvesti pravilo ugovora koje takvu poruku odbija novim kodom i pokriti ga testom.
