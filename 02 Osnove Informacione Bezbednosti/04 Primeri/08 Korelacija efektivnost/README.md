# Vežba 8 — Korelacija i efektivnost kontrole

`EventCorrelator` povezuje događaje u slučajeve. `ControlEffectivenessCalculator` poredi udeo blokiranih pokušaja sa zadatim ciljem; bez pokušaja nema osnova za ocenu efektivnosti.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba08.ConsoleUi
dotnet test Oib.Vezba08.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Kontrola postoji, ali ne blokira potreban udeo rizičnih pokušaja. |
| Testovi | `EventCorrelatorTests`, `ControlEffectivenessCalculatorTests`, `AnalyzeSecurityEventsHandlerTests` |
| Dokaz | `SecurityAnalysisReport` prikazuje povezane slučajeve i rezultate merenja. |

## Zadatak

Dodati vreme događaja i meriti koliko je prošlo od prvog do poslednjeg događaja u slučaju. Proširiti `CorrelationCase` i napisati test za slučaj sa jednim događajem.

[Pregled svih primera](<../README.md>)
