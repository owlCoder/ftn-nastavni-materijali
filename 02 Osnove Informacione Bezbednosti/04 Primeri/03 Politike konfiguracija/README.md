# Vežba 3 — Politike, konfiguracija i vidljivost

`ConfigurationDriftDetector` poredi aktivnu konfiguraciju sa usvojenim baseline-om. Kontrole implementiraju `IConfigurationControl`; nalaz označava slabljenje zaštite.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba03.ConsoleUi
dotnet test Oib.Vezba03.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Minimalna dužina lozinke ili zahtev za administratorski MFA oslabi u odnosu na baseline. |
| Testovi | `ConfigurationControlTests`, `DetectConfigurationDriftHandlerTests` |
| Dokaz | `DriftReport` sadrži correlation ID i verziju baseline-a. |

## Zadatak

Dodati kontrolu za maksimalno trajanje sesije: proširiti `SecurityBaseline` i `SecurityConfiguration`, napisati novu `IConfigurationControl` klasu i test koji pokazuje da duža sesija od dozvoljene daje nalaz.

[Pregled svih primera](<../README.md>)
