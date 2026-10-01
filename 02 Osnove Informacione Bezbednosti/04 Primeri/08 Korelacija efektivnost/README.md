# Vežba 8 — Korelacija, efektivnost i učenje

Primer koreliše više događaja u jedan slučaj i meri da li kontrola daje očekivani ishod, umesto da se samo postojanje kontrole tretira kao dokaz.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba08.ConsoleUi
dotnet test Oib.Vezba08.sln
```

## Struktura

```text
src/
  Oib.Vezba08.Domain/          SecurityEvent, CorrelationCase, EventCorrelator,
                               ControlMeasurement, ControlEffectivenessCalculator
  Oib.Vezba08.Application/     AnalyzeSecurityEventsHandler + port za izvor događaja
  Oib.Vezba08.Infrastructure/  in-memory izvor događaja
  Oib.Vezba08.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba08.Tests/           testovi korelatora, kalkulatora i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | poverljivi izvoz zaštićen step-up kontrolom |
| Pretnja | kontrola postoji, ali ne zaustavlja pokušaje koje treba da zaustavi |
| Zahtev | kontrola mora da blokira najmanje ciljni udeo pokušaja |
| Kontrola | merenje kroz `ControlEffectivenessCalculator` |
| Test | `EventCorrelatorTests`, `ControlEffectivenessCalculatorTests`, `AnalyzeSecurityEventsHandlerTests` |
| Dokaz | `SecurityAnalysisReport` sa slučajevima i izmerenom efektivnošću |

## SOLID i Clean Architecture

- Korelacija i merenje su dva odvojena domenska servisa (SRP).
- `ControlMeasurement` nosi stanje, a računanje je u kalkulatoru; cilj je parametar, ne konstanta.
- Kontrola bez ijednog pokušaja ne proglašava se efektivnom: nema dokaza, nema ispunjenog cilja.
- Handler zavisi od `ISecurityEventSource`, pa se in-memory izvor može zameniti log skladištem (DIP).

## Zadatak

Dodati vreme događaja i meriti koliko je prošlo od prvog do poslednjeg događaja u slučaju. Proširiti `CorrelationCase` i napisati test za slučaj sa jednim događajem.
