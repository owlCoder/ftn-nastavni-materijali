# Vežba 4 — Verzionisanje i failure-first testiranje

`InboxPolicy` odlučuje o kompatibilnosti, starosti i duploj isporuci. Ista major verzija je kompatibilna, a novija minor verzija se prihvata. Prijemna strana vodi evidenciju obrađenih poruka.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba04.ConsoleUi
dotnet test Odp.Vezba04.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Dupla isporuka, prestara poruka i nekompatibilna major verzija. |
| Testovi | `InboxPolicyTests` i `ReceiveMessageHandlerTests` |
| Dokaz | Svaka isporuka dobija kod odluke; duplikat ne ponavlja poslovni efekat. |

## Zadatak

Dodati pravilo da se poruka sa minor verzijom većom od poznate prihvata, ali se ishod označava novim kodom `message_accepted_newer_minor`, kako bi tim znao da ugovor treba pregledati. Pokriti granični slučaj testom.

[Pregled svih primera](<../README.md>)
