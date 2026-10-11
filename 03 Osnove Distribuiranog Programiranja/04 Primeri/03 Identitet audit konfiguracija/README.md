# Vežba 3 — Identitet, audit i konfiguracija

`ScheduleContactHandler` prenosi `OperationContext` kroz zahtev centru, poziv stanice i audit. Validator proverava zahtev prema konfiguraciji; stanica odlučuje o svom rasporedu.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba03.ConsoleUi
dotnet test Odp.Vezba03.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Stanica odbija rezervaciju ili zahtev prelazi konfiguraciono ograničenje. |
| Testovi | Isti `CorrelationId` pojavljuje se u pozivu stanice i audit zapisu. |
| Dokaz | `AuditEntry` postoji za svaki ishod, uključujući odbijanje pre poziva stanice. |

## Zadatak

Dodati u `OperationContext` identifikator pozivajuće komponente (`Source`) i upisati ga u revizijski zapis. Proširiti test tako da dokazuje da zapis sadrži i izvor operacije.

[Pregled svih primera](<../README.md>)
