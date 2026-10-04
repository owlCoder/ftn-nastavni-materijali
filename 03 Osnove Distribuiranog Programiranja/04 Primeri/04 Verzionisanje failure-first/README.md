# Vežba 4 — Verzionisanje i failure-first testiranje

Primer polazi od onoga što mreža sigurno radi: ista poruka može stići dva puta, može zakasniti i može biti poslata novijom verzijom ugovora. Prijemna strana zato za svaku poruku donosi eksplicitnu odluku, a testovi najpre pokrivaju neuspešne puteve.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba04.ConsoleUi
dotnet test Odp.Vezba04.sln
```

## Struktura

```text
src/
  Odp.Vezba04.Domain/          MessageEnvelope, ContractVersion, InboxPolicy, InboxRules
  Odp.Vezba04.Application/     ReceiveMessageHandler + portovi (obrađene poruke, poslovni efekat, sat)
  Odp.Vezba04.Infrastructure/  in-memory evidencija obrađenih poruka, procesor koji beleži efekat, satovi
  Odp.Vezba04.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba04.Tests/           testovi politike prijema i use-case-a
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | prijemna strana je vlasnik evidencije obrađenih poruka |
| Ugovor | `ContractVersion` — ista major verzija je kompatibilna, novija minor se prihvata |
| Odluka | `InboxPolicy` — prihvati, ignoriši duplikat, odbij zakasnelu ili nekompatibilnu poruku |
| Failure scenario | dupla isporuka, poruka starija od dozvoljenog, druga major verzija |
| Test | `InboxPolicyTests` (granice) i `ReceiveMessageHandlerTests` (efekat se dešava jednom) |
| Dokaz | kod odluke za svaku isporuku i broj izvršenih poslovnih efekata |

## SOLID i Clean Architecture

- Pravilo prijema je u `InboxPolicy`; handler samo povezuje politiku, evidenciju i poslovni efekat.
- Vreme je port (`IClock`), pa se kašnjenje testira pomeranjem sata.
- `IMessageProcessor` odvaja poslovni efekat od odluke o prijemu (SRP, DIP).
- Odbijena poruka se ne upisuje u evidenciju, pa kasnija ispravna isporuka nije pogrešno označena kao duplikat.

## Zadatak

Dodati pravilo da se poruka sa minor verzijom većom od poznate prihvata, ali se ishod označava novim kodom `message_accepted_newer_minor`, kako bi tim znao da ugovor treba pregledati. Pokriti granični slučaj testom.
