# Vežba 7 — Atributi, rizik i pregled pristupa

Primer dopunjuje ulogu kontekstom: uređaj, lokacija, osetljivost resursa i nivo rizika menjaju odluku za istog korisnika. Odobren pristup dobija rok za periodični pregled.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba07.ConsoleUi
dotnet test Oib.Vezba07.sln
```

## Struktura

```text
src/
  Oib.Vezba07.Domain/          AccessContext, AccessDecision, IAccessRule i pravila,
                               RiskBasedAccessPolicy, AccessReviewItem
  Oib.Vezba07.Application/     EvaluateAccessHandler + portovi (raspored pregleda, sat)
  Oib.Vezba07.Infrastructure/  in-memory raspored pregleda, sistemski sat
  Oib.Vezba07.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba07.Tests/           testovi pravila, politike i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | ograničen resurs (izvoz izveštaja) |
| Pretnja | ispravna uloga se koristi sa nepoznatog uređaja, lokacije ili uz visok rizik |
| Zahtev | uloga nije dovoljna; kontekst mora da ispuni sva pravila |
| Kontrola | `RiskBasedAccessPolicy` sa lancem `IAccessRule` pravila |
| Test | `AccessRuleTests`, `RiskBasedAccessPolicyTests`, `EvaluateAccessHandlerTests` |
| Dokaz | `AccessDecision` sa kodom i `AccessReviewItem` sa rokom i poslovnim vlasnikom |

## SOLID i Clean Architecture

- Svako pravilo je posebna klasa: `BusinessRoleRule`, `ManagedDeviceRule`, `TrustedLocationRule`, `RiskThresholdRule` (SRP).
- Novi atribut se uvodi novim `IAccessRule` pravilom; politika se ne menja (OCP).
- Prvo pravilo koje odbije zahtev određuje kod, razlog i rok ponovne procene.
- Handler zakazuje pregled samo za odobren pristup, kroz port `IAccessReviewSchedule` (DIP).

## Zadatak

Dodati pravilo radnog vremena: ograničen resurs dostupan je samo radnim danima od 8 do 18 časova. Vreme proslediti kroz kontekst, a ne čitati ga iz sistema unutar pravila.
