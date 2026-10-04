# Vežba 7 — Rad bez veze i pouzdana isporuka

Primer čuva nameru kroz prekid veze. Stanica svako merenje najpre upisuje u lokalni outbox, a slanje je odvojen korak koji se ponavlja dok ne stigne potvrda. Pošto potvrda može da se izgubi i posle uspešne isporuke, ista poruka ponekad stigne dva puta; centar je zato primenjuje samo prvi put.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba07.ConsoleUi
dotnet test Odp.Vezba07.sln
```

## Struktura

```text
src/
  Odp.Vezba07.Domain/          OutboxMessage, OutboxOrdering, InboxFilter
  Odp.Vezba07.Application/     RecordMeasurementHandler, FlushOutboxHandler + portovi (outbox, veza)
  Odp.Vezba07.Infrastructure/  in-memory outbox, simulirana veza sa kvarovima, prijemno sanduče centra
  Odp.Vezba07.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba07.Tests/           testovi redosleda, deduplikacije i celog toka
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | stanica je vlasnik outbox-a; centar je vlasnik evidencije primenjenih poruka |
| Identitet operacije | `MessageId` — isti pri prvom slanju i pri ponavljanju |
| Odluka | slanje staje na prvoj nepotvrđenoj poruci; `InboxFilter` ignoriše ponavljanje |
| Failure scenario | prekid veze; potvrda izgubljena posle isporuke |
| Test | posle povratka veze sve stiže izvornim redom; ponovljena poruka se primenjuje jednom |
| Dokaz | `FlushReport` za svaki pokušaj i broj ignorisanih duplikata u centru |

## SOLID i Clean Architecture

- Upis namere (`RecordMeasurementHandler`) i slanje (`FlushOutboxHandler`) su odvojeni use-case-ovi, pa prekid veze ne zaustavlja rad stanice.
- `IUplink.TrySend` vraća samo da li je potvrda stigla; kod ne može da zaključi da poruka nije isporučena.
- Redosled i deduplikacija su domenska pravila (`OutboxOrdering`, `InboxFilter`), nezavisna od skladišta i veze.
- Kvarovi veze su deo simulatora u `Infrastructure`, pa se zadaju u testu umesto da se čekaju.

## Zadatak

Ograničiti veličinu outbox-a (`Capacity`). Kada je pun, novo merenje se odbija kodom `outbox_full` umesto da se tiho izgubi staro. Pravilo pokriti testom u kome veza ostaje u prekidu.
