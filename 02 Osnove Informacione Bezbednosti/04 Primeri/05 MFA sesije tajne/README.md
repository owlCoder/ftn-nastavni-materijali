# Vežba 5 — MFA i sesije

`StepUpAuthenticationPolicy` proverava da li sesija ima dovoljno svežu MFA potvrdu za rizičnu operaciju. Odluka razlikuje odbijanje i potrebu za novom potvrdom.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba05.ConsoleUi
dotnet test Oib.Vezba05.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Preuzeta, opozvana ili davno potvrđena sesija pokušava poverljiv izvoz. |
| Testovi | `StepUpAuthenticationPolicyTests`, `AuthorizeRiskyOperationHandlerTests` |
| Dokaz | `StepUpDecision` vraća kod odluke i oznaku da li se traži MFA. |

## Zadatak

Dodati maksimalnu starost same sesije: sesija izdata pre više od osam sati odbija se bez obzira na MFA. Uvesti nov kod odluke i test za graničnu vrednost.

[Pregled svih primera](<../README.md>)
