# Vežba 5 — Tok podataka i read modeli

Prihvaćeno merenje upisuje se u `ITelemetryLog`, a `StationViewProjector` ažurira izveden prikaz samo novijim merenjem. `FreshnessPolicy` označava zastarele podatke.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba05.ConsoleUi
dotnet test Odp.Vezba05.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Merenje #2 stiže posle #3; stanica potom prestaje da šalje. |
| Testovi | Zakasnelo merenje ostaje u zapisu, bez vraćanja prikaza na staro stanje. |
| Dokaz | `IngestOutcome.ViewChanged` i `StationStatus.IsStale` prikazuju promenu i svežinu. |

## Zadatak

Dodati use-case `RebuildStationView` koji briše prikaz stanice i ponovo ga izvodi iz zapisa. Testom pokazati da je rezultat isti bez obzira na redosled kojim su merenja prvobitno stigla.

[Pregled svih primera](<../README.md>)
