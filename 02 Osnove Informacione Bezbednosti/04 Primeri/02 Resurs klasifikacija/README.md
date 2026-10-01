# Vežba 2 — Autorizacija nad resursom i klasifikacija podataka

Serverska odluka kombinuje dozvolu, vlasništvo nad konkretnim resursom i klasifikaciju podatka. Promena identifikatora u zahtevu ne sme da omogući horizontalni pristup tuđem zapisu.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba02.ConsoleUi
dotnet test Oib.Vezba02.sln
```

## Struktura

```text
src/
  Oib.Vezba02.Domain/          ProtectedResource, DataClassification, Requester, ResourceReadPolicy
  Oib.Vezba02.Application/     ReadResourceHandler + portovi (repozitorijum resursa, revizijski trag)
  Oib.Vezba02.Infrastructure/  in-memory repozitorijum i revizijski trag
  Oib.Vezba02.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba02.Tests/           testovi politike i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | zapis `rec-42` sa vlasnikom i klasifikacijom |
| Pretnja | korisnik pogađa identifikator tuđeg zapisa |
| Zahtev | odnos subjekta prema resursu proverava se na serveru, za svaki zahtev |
| Kontrola | `ResourceReadPolicy` — dozvola, vlasništvo, pa klasifikacija |
| Test | `ResourceReadPolicyTests`, `ReadResourceHandlerTests` |
| Dokaz | `ResourceAccessAuditEntry` sa ishodom i stabilnim kodom |

## SOLID i Clean Architecture

- Pravilo pristupa je u domenskom servisu `ResourceReadPolicy`; handler samo učitava resurs, poziva pravilo i beleži ishod.
- Odbijen zahtev ne vraća podatke: `ReadResourceResult.Resource` je popunjen samo za ishod `Granted`.
- Handler zavisi od `IProtectedResourceRepository` i `IResourceAccessAuditLog` (DIP).
- Ishod je eksplicitan (`Granted`, `Denied`, `NotFound`) i nosi stabilan kod umesto `bool` vrednosti.

## Zadatak

Uvesti radnju izmene zapisa: menja ga samo vlasnik, bez obzira na `records:read:any`. Dodati novu politiku i use-case pored postojećih, bez menjanja `ResourceReadPolicy`.
