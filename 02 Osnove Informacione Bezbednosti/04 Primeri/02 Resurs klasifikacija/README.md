# Vežba 2 — Autorizacija nad resursom i klasifikacija

`ResourceReadPolicy` proverava dozvolu, vlasništvo i klasifikaciju konkretnog zapisa. Provera se izvršava na serveru za svaki zahtev.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba02.ConsoleUi
dotnet test Oib.Vezba02.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Korisnik pogađa identifikator tuđeg zapisa. |
| Testovi | `ResourceReadPolicyTests`, `ReadResourceHandlerTests` |
| Dokaz | `ResourceAccessAuditEntry` sadrži ishod i stabilan kod odluke. |

## Zadatak

Uvesti radnju izmene zapisa: menja ga samo vlasnik, bez obzira na `records:read:any`. Dodati novu politiku i use-case pored postojećih, bez menjanja `ResourceReadPolicy`.

[Pregled svih primera](<../README.md>)
