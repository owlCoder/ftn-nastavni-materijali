# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## Vežba 7 — Rad bez veze i pouzdana isporuka

Prekid veze ne mora da zaustavi lokalni rad, ali uvodi pitanje šta se desilo dok druga strana nije bila dostupna. Buffer, replay i outbox obrasci čuvaju nameru i istoriju, dok deduplikacija sprečava da ponovna isporuka stvori drugi poslovni efekat.

Pouzdana isporuka nije obećanje da mreža nikada neće pogrešiti. Ona je skup jasnih odgovornosti koji omogućava da se poruka sačuva, ponovo pošalje, objasni ako ne prođe i obradi jednom sa stanovišta poslovnog efekta.

### Sistem koji čuva smisao poruke kroz prekid i ponovni kontakt

- Lokalna namera: nastaje i čuva se
- Outbox ili buffer: čeka sigurnu isporuku
- Primalac: pamti obrađene poruke
- DLQ i replay: vidljiva korekcija neuspeha

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 7.1. Isporuka i obrada su odvojene odgovornosti

- Poruka koja ne prolazi ne nestaje tiho, već dobija vidljiv razlog.
- Ponovljena isporuka je očekivana i primalac je prepoznaje.
- Lokalni kapacitet i politika odbacivanja moraju biti eksplicitni.

> **Pitanje za diskusiju**
>
> Koju poruku biste morali sačuvati tokom prekida veze, a koju biste smeli izgubiti uz jasno obaveštenje? Zašto?

### 7.2. Kako se ova tema vidi u razvoju

Tim najpre odlučuje šta može da se zadrži lokalno, koliko dugo i šta se dešava pri popunjenom kapacitetu. Zatim proverava reconnect scenarije i pokazuje da se poruka može pratiti od lokalne namere do konačnog poslovnog ishoda.

#### Česte greške koje treba prepoznati

- Buffer bez ograničenja i bez politike pri popunjenosti.
- Replay koji zaobilazi istu validaciju kao prvi pokušaj.
- Deduplikacija oslonjena samo na podatak koji se može promeniti.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 7.3. Primer: reconnect posle lokalnog rada

Udaljena komponenta nastavlja lokalni rad dok nema vezu i čuva poruke za kasnije. Kada se ponovo poveže, neke od tih poruka su možda već stigle drugim putem. Pouzdana isporuka zato ne znači da se sve šalje bez provere, već da se poruke mogu identifikovati, ponoviti i obrađivati bez duplog poslovnog efekta.

- Buffer ima ograničenje i jasno pravilo kada više nema prostora.
- Outbox čuva nameru zajedno sa poslovnom promenom koju predstavlja.
- Primalac vodi evidenciju obrađenih poruka, a dead-letter čuva neuspehe za kontrolisan replay.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 7.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 7.5. Šema rada bez veze

### Šema rada bez veze

- Lokalna promena: nastaje bez mreže
- Buffer ili outbox: čuva poruku i razlog
- Reconnect: ponavlja isporuku
- Inbox i rezultat: sprečavaju dupli efekat

Prekid veze menja trenutak isporuke, ali ne sme da izbriše nameru niti da je proizvoljno udvostruči.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 7.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Lokalna namera | nastaje i čuva se | Šta se dešava kada lokalna namera nije dostupan ili daje zastarelu informaciju? |
| Outbox ili buffer | čeka sigurnu isporuku | Šta se dešava kada outbox ili buffer nije dostupan ili daje zastarelu informaciju? |
| Primalac | pamti obrađene poruke | Šta se dešava kada primalac nije dostupan ili daje zastarelu informaciju? |
| DLQ i replay | vidljiva korekcija neuspeha | Šta se dešava kada dlq i replay nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 7.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Poruka koja ne prolazi ne nestaje tiho, već dobija vidljiv razlog.
2. Ponovljena isporuka je očekivana i primalac je prepoznaje.
3. Lokalni kapacitet i politika odbacivanja moraju biti eksplicitni.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koju poruku biste morali sačuvati tokom prekida veze, a koju biste smeli izgubiti uz jasno obaveštenje? Zašto?

### 7.8. Izvršivi primer

Primer čuva nameru kroz prekid veze. Stanica svako merenje najpre upisuje u lokalni outbox, a slanje je odvojen korak koji se ponavlja dok ne stigne potvrda. Pošto potvrda može da se izgubi i posle uspešne isporuke, ista poruka ponekad stigne dva puta; centar je zato primenjuje samo prvi put.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-07-rad-bez-veze-outbox
dotnet run --project src/Odp.Vezba07.ConsoleUi
dotnet test Odp.Vezba07.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-07-rad-bez-veze-outbox/src/Odp.Vezba07.Application/Flushing/FlushOutboxHandler.cs

```csharp
public FlushReport Flush()
{
    var pending = ordering.PendingInOrder(outbox.All());
    if (pending.Count == 0)
        return new(0, 0, OutboxCodes.NothingPending);

    var delivered = 0;
    foreach (var message in pending)
    {
        if (!uplink.TrySend(message))
            return new(delivered, pending.Count - delivered, OutboxCodes.DeliveryUnconfirmed);

        outbox.MarkDelivered(message.Position);
        delivered++;
    }

    return new(delivered, 0, OutboxCodes.Flushed);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-07-rad-bez-veze-outbox/tests/Odp.Vezba07.Tests/Application/OutboxFlowTests.cs

```csharp
[Test]
public void Flush_WhenAckIsLost_ResendsAndTheCenterAppliesTheMessageOnce()
{
    _record.Record("m-1", "a");
    _record.Record("m-2", "b");
    _uplink.LoseNextAck = true;

    var first = _flush.Flush();
    var second = _flush.Flush();

    using (Assert.EnterMultipleScope())
    {
        Assert.That(first, Is.EqualTo(new FlushReport(0, 2, OutboxCodes.DeliveryUnconfirmed)));
        Assert.That(second, Is.EqualTo(new FlushReport(2, 0, OutboxCodes.Flushed)));
        Assert.That(_center.AppliedPayloads, Is.EqualTo(new[] { "a", "b" }));
        Assert.That(_center.DuplicatesIgnored, Is.EqualTo(1));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba07.ConsoleUi

```text
Veza je u prekidu.
  merenje m-1 -> message_queued
  merenje m-2 -> message_queued
  merenje m-3 -> message_queued
  pokušaj slanja bez veze -> delivery_unconfirmed, isporučeno 0, preostalo 3
Veza se vratila, ali se prva potvrda gubi.
  prvi pokušaj posle povratka -> delivery_unconfirmed, isporučeno 0, preostalo 3
  drugi pokušaj -> outbox_flushed, isporučeno 3, preostalo 0

Centar je primenio 3 poruke redom:
  signal -91 dBm
  signal -88 dBm
  signal -97 dBm
Ignorisanih duplikata: 1
```

#### Šta treba uočiti

- Upis merenja uspeva i kada veze nema; slanje je odvojen korak.
- Posle povratka veze poruke stižu redom kojim su nastale.
- Izgubljena potvrda izaziva ponovno slanje, a centar poruku primenjuje jednom.

> **Zadatak uz primer**
>
> Ograničiti veličinu outbox-a (`Capacity`). Kada je pun, novo merenje se odbija kodom `outbox_full` umesto da se tiho izgubi staro. Pravilo pokriti testom u kome veza ostaje u prekidu.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Rad bez veze i pouzdana isporuka". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.
