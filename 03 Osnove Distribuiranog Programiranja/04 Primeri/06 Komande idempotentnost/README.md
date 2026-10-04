# Vežba 6 — Komande, poslovi i idempotentnost

Primer vodi komandu ka udaljenom uređaju kroz neizvesnost. Slanje nema povratnu vrednost: potvrda stiže odvojeno, kasni ili ne stigne. Zato komanda ima stabilan identifikator, ponovno slanje nosi isti identifikator, uređaj ga izvršava jednom, a potvrda koja stigne posle isteka i dalje ispravlja stanje.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba06.ConsoleUi
dotnet test Odp.Vezba06.sln
```

## Struktura

```text
src/
  Odp.Vezba06.Domain/          DeviceCommand, CommandLifecycle, RetryRules, CommandCodes
  Odp.Vezba06.Application/     DispatchCommandHandler, RetryTimedOutCommandsHandler, AcknowledgeCommandHandler + portovi
  Odp.Vezba06.Infrastructure/  in-memory skladište komandi, simulirani uređaj, satovi
  Odp.Vezba06.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba06.Tests/           testovi životnog ciklusa komande i celog toka
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | centar je vlasnik stanja komande; uređaj je vlasnik izvršenja |
| Identitet operacije | `CommandId` — isti za prvi pokušaj, ponovno slanje i potvrdu |
| Odluka | `CommandLifecycle` — istek, ponovni pokušaj, odustajanje, potvrda |
| Failure scenario | izgubljena potvrda, iscrpljeni pokušaji, potvrda posle odustajanja |
| Test | dupli zahtev šalje jednom; dve isporuke daju jedno izvršenje; zakasnela potvrda se usklađuje |
| Dokaz | kod ishoda za svaki korak, broj isporuka i broj izvršenja na uređaju |

## SOLID i Clean Architecture

- Pravila životnog ciklusa su u `CommandLifecycle`; handler-i samo povezuju skladište, vezu i sat.
- `IDeviceLink.Send` nema povratnu vrednost, pa kod ne može da pretpostavi sinhronu potvrdu.
- Vreme je port (`IClock`), pa se istek testira pomeranjem sata.
- Svaki use-case ima jednu odgovornost: slanje, ponovni pokušaj i potvrda su odvojeni (SRP).

## Zadatak

Dodati odlaganje između pokušaja (`RetryDelay`) koje raste sa brojem pokušaja. Pravilo dodati u `CommandLifecycle` i testom pokazati da se ponovno slanje ne dešava pre isteka odlaganja.
