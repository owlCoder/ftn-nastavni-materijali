# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## Vežba 4 — Verzionisanje i failure-first testiranje

U distribuiranom sistemu normalan scenario nikada nije cela priča. Poruka može da stigne dva puta, van redosleda ili nakon isteka roka. Verzije ugovora i kontrolisani failure scenariji omogućavaju timu da te situacije prouči dok su bezbedne i razumljive.

Failure-first pristup ne predviđa svaki mogući kvar. On podstiče tim da za važnu operaciju postavi prava pitanja: šta ako odgovor kasni, šta ako se zahtev ponovi i šta ako komponenta nestane između dve radnje?

### Razvoj koji računa na kašnjenje, duplikate i prekide

- Promena: novi ugovor ili ponašanje
- Kompatibilnost: šta stari korisnici razumeju
- Failure scenario: kontrolisana smetnja
- Test: dokaz očekivanog ishoda

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 4.1. Neizvesnost je deo ugovora

- Kompatibilnost se procenjuje prema primaocima poruke, ne samo prema kodu pošiljaoca.
- Kašnjenje i duplikat su normalni ulazi koje komponenta mora da objasni.
- Test treba da proveri vidljiv ishod, ne samo da metoda ne baca izuzetak.

> **Pitanje za diskusiju**
>
> Za koju operaciju bi kasno pristigla poruka bila opasnija od potpuno izgubljene poruke i zašto?

### 4.2. Kako se ova tema vidi u razvoju

Za najvažnije tokove tim formuliše nekoliko malih failure eksperimenata i pokreće ih u istom okruženju. Rezultat ulazi u testnu bazu i u opis odluke, pa sledeća promena ne mora ponovo da otkriva ista ograničenja.

#### Česte greške koje treba prepoznati

- Pretpostavka da se poruka isporučuje tačno jednom.
- Breaking promena označena kao bezopasna jer lokalni testovi prolaze.
- Testiranje prekida tek nakon integracije svih komponenti.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 4.3. Primer: potvrda stiže nakon isteka roka

Komponenta čeka odgovor do isteka roka i pokreće bezbedan oporavak. Zatim stigne stara potvrda iz prvog pokušaja. Ako sistem nema stabilan identitet operacije i pravilo za redosled, ta potvrda može prepisati novije stanje. Failure scenario zato nije izuzetak koji se samo uhvati, već deo ugovora o ponašanju.

- Timeout znači da odgovor nije stigao na vreme, ne da se radnja sigurno nije izvršila.
- Duplikat i kasna poruka moraju se razdvojiti od nove legitimne operacije.
- Ponovljiv simulator omogućava da se isti redosled greške proveri nakon svake izmene.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 4.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 4.5. Šema failure-first provere

### Šema failure-first provere

- Normalan tok: zahtev i očekivan odgovor
- Kontrolisana smetnja: kašnjenje, duplikat ili prekid
- Vidljiv ishod: stanje koje sistem zadržava
- Test: dokaz ponovljivog ponašanja

Cilj nije pretpostaviti savršen transport, već jasno definisati ponašanje kada transport nije savršen.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 4.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Promena | novi ugovor ili ponašanje | Šta se dešava kada promena nije dostupan ili daje zastarelu informaciju? |
| Kompatibilnost | šta stari korisnici razumeju | Šta se dešava kada kompatibilnost nije dostupan ili daje zastarelu informaciju? |
| Failure scenario | kontrolisana smetnja | Šta se dešava kada failure scenario nije dostupan ili daje zastarelu informaciju? |
| Test | dokaz očekivanog ishoda | Šta se dešava kada test nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 4.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Kompatibilnost se procenjuje prema primaocima poruke, ne samo prema kodu pošiljaoca.
2. Kašnjenje i duplikat su normalni ulazi koje komponenta mora da objasni.
3. Test treba da proveri vidljiv ishod, ne samo da metoda ne baca izuzetak.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Za koju operaciju bi kasno pristigla poruka bila opasnija od potpuno izgubljene poruke i zašto?

### 4.8. Izvršivi primer

Primer polazi od onoga što mreža sigurno radi: ista poruka može stići dva puta, može zakasniti i može biti poslata novijom verzijom ugovora. Prijemna strana zato za svaku poruku donosi eksplicitnu odluku, a testovi najpre pokrivaju neuspešne puteve.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-04-verzionisanje-failure-first
dotnet run --project src/Odp.Vezba04.ConsoleUi
dotnet test Odp.Vezba04.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-04-verzionisanje-failure-first/src/Odp.Vezba04.Domain/Messaging/InboxPolicy.cs

```csharp
public InboxDecision Decide(MessageEnvelope envelope, bool alreadyProcessed, DateTimeOffset now)
{
    ArgumentNullException.ThrowIfNull(envelope);

    if (envelope.Version.Major != rules.SupportedMajor)
        return new(false, InboxCodes.UnsupportedMajorVersion);
    if (alreadyProcessed)
        return new(false, InboxCodes.Duplicate);
    if (now - envelope.SentAt > rules.MaxAge)
        return new(false, InboxCodes.TooOld);

    return new(true, InboxCodes.Accepted);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-04-verzionisanje-failure-first/tests/Odp.Vezba04.Tests/Application/ReceiveMessageHandlerTests.cs

```csharp
[Test]
public void Receive_WhenTheSameMessageIsDeliveredTwice_ProcessesItOnce()
{
    var envelope = Envelope("m-1", 1, Start);

    var first = _handler.Receive(envelope);
    var second = _handler.Receive(envelope);

    using (Assert.EnterMultipleScope())
    {
        Assert.That(first.Code, Is.EqualTo(InboxCodes.Accepted));
        Assert.That(second.Code, Is.EqualTo(InboxCodes.Duplicate));
        Assert.That(_processor.ProcessedPayloads, Has.Count.EqualTo(1));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba04.ConsoleUi

```text
m-1 v1.0 (prva isporuka) -> message_accepted
m-1 v1.0 (ponovljena isporuka iste poruke) -> message_duplicate
m-2 v1.3 (novija minor verzija) -> message_accepted
m-3 v2.0 (druga major verzija) -> message_major_version_unsupported
m-4 v1.0 (poruka koja je putovala 12 minuta) -> message_too_old

Poslovni efekat se dogodio 2 puta:
  telemetrija m-1
  telemetrija m-2
```

#### Šta treba uočiti

- Duplikat, zakasnela poruka i druga major verzija su očekivani ulazi sa sopstvenim kodom.
- Poslovni efekat se dešava jednom, koliko god puta da ista poruka stigne.
- Novija minor verzija se prihvata; druga major verzija se odbija.

> **Zadatak uz primer**
>
> Dodati pravilo da se poruka sa minor verzijom većom od poznate prihvata, ali se ishod označava novim kodom `message_accepted_newer_minor`, kako bi tim znao da ugovor treba pregledati. Pokriti granični slučaj testom.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Verzionisanje i failure-first testiranje". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P2 — Identitet, audit, observability i konfiguracija

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.
