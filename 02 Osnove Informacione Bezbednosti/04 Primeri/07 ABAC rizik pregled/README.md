# Vežba 7 — ABAC, rizik i pregled pristupa

`RiskBasedAccessPolicy` proverava kontekst kroz skup `IAccessRule` pravila. Pristup zahteva odgovarajuću ulogu, uređaj, lokaciju i prihvatljiv rizik.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba07.ConsoleUi
dotnet test Oib.Vezba07.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Ispravna uloga koristi se sa nepoznatog uređaja ili uz visok rizik. |
| Testovi | `AccessRuleTests`, `RiskBasedAccessPolicyTests`, `EvaluateAccessHandlerTests` |
| Dokaz | `AccessDecision` daje razlog odluke; `AccessReviewItem` određuje rok i vlasnika pregleda. |

## Zadatak

Dodati pravilo radnog vremena: ograničen resurs dostupan je samo radnim danima od 8 do 18 časova. Vreme proslediti kroz kontekst, a ne čitati ga iz sistema unutar pravila.

[Pregled svih primera](<../README.md>)
