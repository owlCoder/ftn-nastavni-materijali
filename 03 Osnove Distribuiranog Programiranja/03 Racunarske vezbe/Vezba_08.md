# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## Vežba 8 — Koordinacija, protok i eventualna konzistentnost

Napredni distribuirani dizajn prihvata da različite komponente mogu privremeno imati različit pogled na stanje. Izazov nije da se ta činjenica sakrije, već da se vlasništvo, prebacivanje rada, opterećenje i svežina podataka modeluju tako da korisnik i operater znaju šta sistem može da garantuje.

Koordinacija sprečava sukob nad ekskluzivnim radom, backpressure štiti sistem kada obrada zaostaje, a replike ubrzavaju čitanje uz cenu privremene zastarelosti. Svaki mehanizam ima korist i cenu koju tim mora umeti da objasni.

### Svesni kompromisi kada sistem raste i radi pod pritiskom

- Vlasništvo: jedan važeći izvršilac
- Opterećenje: ritam proizvodnje i obrade
- Replika: brže čitanje uz kašnjenje
- Jasan ugovor: vidljiva ograničenja

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 8.1. Konzistentnost je odluka o garanciji

- Fencing token sprečava da stari vlasnik nastavi rad nakon preuzimanja.
- Backpressure bira kontrolisano usporavanje ili ograničen gubitak pre potpunog kolapsa.
- Read model može biti zastareo, ali sistem mora pokazati koliko i kada to utiče na odluku.

> **Pitanje za diskusiju**
>
> Za jednu odluku navedite da li zahteva najnovije stanje ili može da koristi zastareo prikaz, pa objasnite posledicu izbora.

### 8.2. Kako se ova tema vidi u razvoju

Završna odbrana ne traži savršenu arhitekturu. Traži jasan trade-off: koje stanje mora biti trenutno, gde je prihvatljivo kašnjenje, kako se sprečava stari vlasnik i šta sistem radi kada ne može da prati ulazni ritam.

#### Česte greške koje treba prepoznati

- Failover pokrenut na osnovu jednog kratkog prekida.
- Neograničen red poruka koji samo odlaže kvar.
- Korišćenje zastarelog read modela kao da je autoritativno stanje.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 8.3. Primer: dva čvora i jedan ekskluzivan posao

Dve instance nakratko izgube vezu jedna sa drugom. Obe mogu misliti da smeju da nastave isti ekskluzivan posao. Fencing token ili epoha daje novom vlasniku jači dokaz vlasništva, pa sistem može odbiti zakašneli pokušaj stare instance čak i kada se ona kasnije ponovo pojavi.

- Koordinacija bira jednog važećeg izvršioca za rad koji ne sme biti paralelan.
- Backpressure odlučuje kako sistem reaguje kada ulazni ritam nadmaši obradu.
- Replika ubrzava čitanje, ali njen prikaz mora moći da pokaže zastarelost.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 8.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 8.5. Šema rada pod pritiskom

### Šema rada pod pritiskom

- Vlasništvo: jedan važeći izvršilac
- Opterećenje: kontrolisan ritam obrade
- Replika: brže čitanje uz kašnjenje
- Jasan ugovor: vidljive granice garancije

Napredni distribuirani dizajn ne skriva kompromis, već korisniku i operateru jasno pokazuje garanciju.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 8.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Vlasništvo | jedan važeći izvršilac | Šta se dešava kada vlasništvo nije dostupan ili daje zastarelu informaciju? |
| Opterećenje | ritam proizvodnje i obrade | Šta se dešava kada opterećenje nije dostupan ili daje zastarelu informaciju? |
| Replika | brže čitanje uz kašnjenje | Šta se dešava kada replika nije dostupan ili daje zastarelu informaciju? |
| Jasan ugovor | vidljiva ograničenja | Šta se dešava kada jasan ugovor nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 8.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Fencing token sprečava da stari vlasnik nastavi rad nakon preuzimanja.
2. Backpressure bira kontrolisano usporavanje ili ograničen gubitak pre potpunog kolapsa.
3. Read model može biti zastareo, ali sistem mora pokazati koliko i kada to utiče na odluku.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Za jednu odluku navedite da li zahteva najnovije stanje ili može da koristi zastareo prikaz, pa objasnite posledicu izbora.

### 8.8. Izvršivi primer

Primer pokazuje dve garancije koje sistem mora da izabere kada raste. Prva je vlasništvo: samo jedan worker sme da menja raspored stanice, vlasništvo je vremenski ograničeno (lease), a resurs odbija upis bivšeg vlasnika po zastarelom tokenu. Druga je protok: kada je red pun, pošiljalac dobija jasan odgovor umesto da posao neograničeno čeka.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-08-koordinacija-protok
dotnet run --project src/Odp.Vezba08.ConsoleUi
dotnet test Odp.Vezba08.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-08-koordinacija-protok/src/Odp.Vezba08.Domain/Leases/LeasePolicy.cs

```csharp
public LeaseDecision Acquire(Lease? current, string resource, string candidateId, DateTimeOffset now)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(resource);
    ArgumentException.ThrowIfNullOrWhiteSpace(candidateId);

    if (current is null || now >= current.ExpiresAt)
    {
        var token = (current?.FencingToken ?? 0) + 1;
        return new(true, LeaseCodes.Granted, new Lease(resource, candidateId, token, now + _duration));
    }

    return current.OwnerId == candidateId
        ? new(true, LeaseCodes.Renewed, current with { ExpiresAt = now + _duration })
        : new(false, LeaseCodes.HeldByAnother, current);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-08-koordinacija-protok/tests/Odp.Vezba08.Tests/Application/CoordinationFlowTests.cs

```csharp
[Test]
public void Write_WhenFormerOwnerWakesUpAfterTakeover_IsRejectedByItsStaleToken()
{
    var former = _acquire.Acquire(Resource, "worker-a").Lease;
    _clock.Advance(TimeSpan.FromSeconds(45));
    var current = _acquire.Acquire(Resource, "worker-b").Lease;
    _write.Write(Resource, current.FencingToken, "plan B");

    var code = _write.Write(Resource, former.FencingToken, "zakasneli plan A");

    using (Assert.EnterMultipleScope())
    {
        Assert.That(code, Is.EqualTo(FencingCodes.StaleToken));
        Assert.That(_schedules.ValueOf(Resource), Is.EqualTo("plan B"));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba08.ConsoleUi

```text
Koordinacija:
  worker-a traži lease -> lease_granted (vlasnik worker-a, token 1)
  worker-b traži lease -> lease_held_by_another (vlasnik worker-a, token 1)
  worker-a upisuje "plan A" sa tokenom 1 -> write_accepted
  worker-a je zastao 45 s; lease je istekao
  worker-b traži lease -> lease_granted (vlasnik worker-b, token 2)
  worker-b upisuje "plan B" sa tokenom 2 -> write_accepted
  worker-a upisuje "zakasneli plan A" sa tokenom 1 -> stale_fencing_token
  važeći raspored: plan B

Protok (kapacitet reda 2):
  job-1 -> job_accepted
  job-2 -> job_accepted
  job-3 -> queue_overloaded
```

#### Šta treba uočiti

- U jednom trenutku samo jedan worker je vlasnik resursa.
- Posle isteka lease-a novi vlasnik dobija veći token, a upis bivšeg vlasnika je odbijen.
- Kada je red pun, pošiljalac dobija `queue_overloaded` umesto tihog čekanja.

> **Zadatak uz primer**
>
> Dodati u prikaz rasporeda podatak o svežini: vreme poslednjeg upisa i oznaku `IsStale` kada je prikaz stariji od zadatog praga. Testom pokazati da čitalac vidi zastareo, ali jasno označen podatak dok vlasnik ne upiše novi.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Koordinacija, protok i eventualna konzistentnost". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P4 — Operativni i napredni nivo — završna odbrana

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.
