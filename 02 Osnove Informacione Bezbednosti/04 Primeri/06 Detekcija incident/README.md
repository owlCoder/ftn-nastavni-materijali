# Vežba 6 — Detekcija i incident

`RepeatedFailedLoginRule` prepoznaje najmanje tri neuspešne prijave u deset minuta. `IncidentFactory` iz signala formira incident sa određenom ozbiljnošću.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba06.ConsoleUi
dotnet test Oib.Vezba06.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Uzastopni pokušaji prijave sa iste adrese. |
| Testovi | `RepeatedFailedLoginRuleTests`, `IncidentFactoryTests`, `DetectIncidentsHandlerTests` |
| Dokaz | `Incident` sadrži identifikator, ozbiljnost, sažetak i vreme otvaranja. |

## Zadatak

Dodati pravilo koje prepoznaje password spraying: ista adresa neuspešno pokušava prijavu na više različitih naloga. Napisati novu `IDetectionRule` klasu i registrovati je u composition root-u.

[Pregled svih primera](<../README.md>)
