# Vežba 6 — Detekcija, incident i ranjivosti

Primer pretvara više neuspešnih prijava u signal sa kontekstom, a zatim u incident koji se može dodeliti i pratiti.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba06.ConsoleUi
dotnet test Oib.Vezba06.sln
```

## Struktura

```text
src/
  Oib.Vezba06.Domain/          LoginAttempt, SecuritySignal, IDetectionRule, RepeatedFailedLoginRule,
                               Incident, IncidentSeverityPolicy, IncidentFactory
  Oib.Vezba06.Application/     DetectIncidentsHandler + portovi (izvor prijava, incidenti, ID, sat)
  Oib.Vezba06.Infrastructure/  in-memory izvor i repozitorijum, sekvencijalni ID, sistemski sat
  Oib.Vezba06.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba06.Tests/           testovi pravila detekcije, fabrike incidenata i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | korisnički nalozi |
| Pretnja | pogađanje lozinke uzastopnim pokušajima sa iste adrese |
| Zahtev | tri ili više neuspešnih prijava u deset minuta otvara incident |
| Kontrola | `RepeatedFailedLoginRule` i `IncidentFactory` |
| Test | `RepeatedFailedLoginRuleTests`, `IncidentFactoryTests`, `DetectIncidentsHandlerTests` |
| Dokaz | `Incident` sa identifikatorom, ozbiljnošću, sažetkom i vremenom otvaranja |

## SOLID i Clean Architecture

- Pravilo detekcije je iza `IDetectionRule`; novo pravilo se dodaje bez menjanja handler-a (OCP).
- Prag, vremenski prozor i granica visoke ozbiljnosti su parametri, ne konstante u kodu.
- Identifikator incidenta i vreme stižu kroz portove `IIncidentIdGenerator` i `IClock`, pa je fabrika čista funkcija (DIP).
- Pravilo prijavljuje svaku sumnjivu kombinaciju naloga i adrese, ne samo najaktivniju.

## Zadatak

Dodati pravilo koje prepoznaje prskanje lozinki: ista adresa neuspešno pokušava prijavu na više različitih naloga. Napisati novu `IDetectionRule` klasu i registrovati je u composition root-u.
