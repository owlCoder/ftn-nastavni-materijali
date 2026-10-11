# Vežba 6 — Komande i idempotentnost

Centar upravlja stanjem komande, a uređaj njenim izvršenjem. `CommandId` ostaje isti pri slanju, ponavljanju i potvrdi; `CommandLifecycle` vodi istek, retry i usklađivanje potvrde.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba06.ConsoleUi
dotnet test Odp.Vezba06.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Izgubljena potvrda, iscrpljeni pokušaji i potvrda posle odustajanja. |
| Testovi | Dupli zahtev šalje jednom; dve isporuke daju jedno izvršenje; kasna potvrda se usklađuje. |
| Dokaz | Kodovi ishoda, broj isporuka i broj izvršenja na uređaju. |

## Zadatak

Dodati odlaganje između pokušaja (`RetryDelay`) koje raste sa brojem pokušaja. Pravilo dodati u `CommandLifecycle` i testom pokazati da se ponovno slanje ne dešava pre isteka odlaganja.

[Pregled svih primera](<../README.md>)
