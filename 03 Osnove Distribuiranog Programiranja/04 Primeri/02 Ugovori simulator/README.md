# Vežba 2 — Ugovori, simulatori i ponovljivost

`TelemetryContractValidator` proverava poruku `TelemetryMessage` verzije `1.0` i dozvoljeni opseg signala. Simulator generiše ponovljiv scenario; stanica je vlasnik merenja, a centar odluke o prijemu.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba02.ConsoleUi
dotnet test Odp.Vezba02.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Svaka N-ta poruka krši ugovor (`signal_out_of_range`). |
| Testovi | Isti scenario daje iste poruke; neispravne poruke odbijaju se po rednom broju. |
| Dokaz | `IngestionReport` navodi prihvaćene i odbijene poruke. |

## Zadatak

Dodati u scenario kašnjenje poruke (`DelayEvery`), tako da svaka N-ta poruka nosi `MeasuredAt` stariji od prethodne. Uvesti pravilo ugovora koje takvu poruku odbija novim kodom i pokriti ga testom.

[Pregled svih primera](<../README.md>)
