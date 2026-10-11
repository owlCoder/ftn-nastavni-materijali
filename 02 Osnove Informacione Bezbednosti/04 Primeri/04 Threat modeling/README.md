# Vežba 4 — Imovina, granice poverenja i threat modeling

Procena scenarija pretnje povezuje imovinu, tok podataka i granicu poverenja. `ThreatRiskCalculator` računa rizik; demonstraciona politika zahteva tretman pri vrednosti 24 ili većoj.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba04.ConsoleUi
dotnet test Oib.Vezba04.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Izvoz ličnih podataka stiže neovlašćenom primaocu. |
| Testovi | `ThreatRiskCalculatorTests`, `AssessThreatModelHandlerTests` |
| Dokaz | `ThreatModelReport` prikazuje scenarije po riziku i predložene kontrole. |

## Zadatak

Dodati rezidualni rizik: `ThreatScenario` dobija procenu efikasnosti kontrole, a izveštaj prikazuje rizik pre i posle kontrole. Pravilo smestiti u domenski servis i pokriti graničnim testovima.

[Pregled svih primera](<../README.md>)
