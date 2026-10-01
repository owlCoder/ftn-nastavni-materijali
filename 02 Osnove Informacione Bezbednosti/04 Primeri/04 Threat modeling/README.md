# Vežba 4 — Imovina, granice poverenja i threat modeling

Primer modeluje vrednu imovinu i tok podataka koji prelazi granicu poverenja, a zatim prioritizuje scenarije pretnji prema verovatnoći i posledici.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba04.ConsoleUi
dotnet test Oib.Vezba04.sln
```

## Struktura

```text
src/
  Oib.Vezba04.Domain/          Asset, DataFlow, ThreatScenario, ThreatScenarioValidator,
                               RiskPolicy i ThreatRiskCalculator
  Oib.Vezba04.Application/     AssessThreatModelHandler + port za scenarije
  Oib.Vezba04.Infrastructure/  in-memory repozitorijum scenarija
  Oib.Vezba04.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba04.Tests/           testovi validatora, kalkulatora i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | lični podaci, poslovna posledica 5 |
| Pretnja | izvoz stiže neovlašćenom primaocu preko granice poverenja |
| Zahtev | scenario sa rizikom 24 ili više mora dobiti tretman |
| Kontrola | potpisan zahtev i enkripcija primaoca (`ProposedControl`) |
| Test | `ThreatRiskCalculatorTests`, `AssessThreatModelHandlerTests` |
| Dokaz | `ThreatModelReport` sa scenarijima poređanim po riziku |

Rizik se računa kao `verovatnoća × posledica × faktor granice poverenja`. Skala je 1–5, a faktor i prag tretmana su deo `RiskPolicy` objekta, ne brojevi rasuti po kodu.

## SOLID i Clean Architecture

- Model (`Asset`, `DataFlow`, `ThreatScenario`) nosi stanje; `ThreatScenarioValidator` proverava skalu, a `ThreatRiskCalculator` računa rizik (SRP).
- Neispravan scenario nije izuzetak: vraća se kao `RejectedScenario` sa stabilnim kodom.
- Promena praga ili faktora je promena konfiguracije `RiskPolicy`, ne izmena kalkulatora (OCP).
- Handler zavisi od `IThreatScenarioRepository` (DIP).

## Zadatak

Dodati rezidualni rizik: `ThreatScenario` dobija procenu efikasnosti kontrole, a izveštaj prikazuje rizik pre i posle kontrole. Pravilo smestiti u domenski servis i pokriti graničnim testovima.
