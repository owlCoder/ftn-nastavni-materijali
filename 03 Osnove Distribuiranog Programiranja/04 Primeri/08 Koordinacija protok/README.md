# Vežba 8 — Koordinacija i kontrola protoka

`LeasePolicy` dodeljuje vremenski ograničeno vlasništvo. Resurs proverava `FencingToken` da bi odbio starog vlasnika; `IntakePolicy` ograničava prijem poslova u red.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba08.ConsoleUi
dotnet test Odp.Vezba08.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Stari vlasnik nastavlja rad posle isteka lease-a; red poslova je pun. |
| Testovi | Upis sa starim tokenom je odbijen; treći posao dobija `queue_overloaded`. |
| Dokaz | Raspored ostaje važeći posle preuzimanja; svaki zahtev dobija kod odgovora. |

## Zadatak

Dodati u prikaz rasporeda podatak o svežini: vreme poslednjeg upisa i oznaku `IsStale` kada je prikaz stariji od zadatog praga. Testom pokazati da čitalac vidi zastareo, ali jasno označen podatak dok vlasnik ne upiše novi.

[Pregled svih primera](<../README.md>)
