# Vežba 3 — Politike, konfiguracija i vidljivost

Primer poredi aktivnu konfiguraciju sa verzionisanim referentnim stanjem (baseline) i prijavljuje odstupanja uz correlation ID, tako da se nalaz može pratiti kroz zapise.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba03.ConsoleUi
dotnet test Oib.Vezba03.sln
```

## Struktura

```text
src/
  Oib.Vezba03.Domain/          SecurityBaseline, SecurityConfiguration, IConfigurationControl,
                               kontrole i ConfigurationDriftDetector
  Oib.Vezba03.Application/     DetectConfigurationDriftHandler + portovi
  Oib.Vezba03.Infrastructure/  in-memory izvori konfiguracije, generator correlation ID-a, tekstualni zapis
  Oib.Vezba03.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba03.Tests/           testovi kontrola i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | bezbednosna konfiguracija sistema |
| Pretnja | konfiguracija neprimetno oslabi u odnosu na usvojenu politiku |
| Zahtev | svako slabljenje u odnosu na baseline mora biti otkriveno i vidljivo |
| Kontrola | `ConfigurationDriftDetector` sa skupom `IConfigurationControl` provera |
| Test | `ConfigurationControlTests`, `DetectConfigurationDriftHandlerTests` |
| Dokaz | `DriftReport` sa correlation ID-jem i verzijom baseline-a |

## SOLID i Clean Architecture

- Svaka kontrola je jedna klasa (`MinimumPasswordLengthControl`, `AdminMfaControl`) sa jednom proverom (SRP).
- Nova kontrola se dodaje novom `IConfigurationControl` implementacijom i registracijom u composition root-u; detektor se ne menja (OCP).
- Handler zavisi od portova za baseline, aktivnu konfiguraciju, correlation ID i zapis izveštaja (DIP).
- Kontrola prijavljuje samo slabljenje: stroža konfiguracija od baseline-a nije odstupanje.

## Zadatak

Dodati kontrolu za maksimalno trajanje sesije: proširiti `SecurityBaseline` i `SecurityConfiguration`, napisati novu `IConfigurationControl` klasu i test koji pokazuje da duža sesija od dozvoljene daje nalaz.
