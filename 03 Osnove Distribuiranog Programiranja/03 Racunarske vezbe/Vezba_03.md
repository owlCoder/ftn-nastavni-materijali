# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## Vežba 3 — Identitet, audit, observability i konfiguracija

Distribuirani sistem mora moći da odgovori na četiri pitanja: ko je pokrenuo radnju, šta se dogodilo, kroz koje komponente je tok prošao i pod kojim uslovima je radio. Identitet, audit, observability i konfiguracija daju odgovore koji se ne smeju izmišljati iznova u svakoj funkcionalnosti.

Kada se tok pokvari, pojedinačni logovi nisu dovoljni. Zajednički korelacioni identifikator i jasan audit trag omogućavaju da tim sastavi priču o operaciji bez nagađanja i bez izlaganja osetljivih podataka.

### Poprečne mogućnosti koje svaka komponenta koristi na isti način

- Identitet: ko pokreće radnju
- Korelacija: jedan tok kroz komponente
- Audit: poslovno značajna odluka
- Konfiguracija: uslovi rada sistema

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 3.1. Vidljivost mora biti ugrađena

- Audit čuva odluke koje su važne za odgovornost i istragu.
- Logovi i tragovi čuvaju tehnički kontekst, ali ne tajne.
- Konfiguracija ima validno značenje, vlasnika i način promene.

> **Pitanje za diskusiju**
>
> Kako biste rekonstruisali jednu neuspelu operaciju koja je prošla kroz API, red poruka i worker, a da ne čitate neograničenu količinu logova?

### 3.2. Kako se ova tema vidi u razvoju

Tim bira nekoliko operacija koje mora umeti da prati od zahteva do ishoda. Svaka komponenta prenosi zajednički kontekst, a testovi proveravaju da promena konfiguracije ili prekid toka ostavlja dovoljno informacija za razumevanje.

#### Česte greške koje treba prepoznati

- Log koji otkriva kredencijale ili lične podatke.
- Korelacioni identifikator kreiran u svakoj komponenti umesto prenet.
- Konfiguracija prihvaćena bez provere opsega i uticaja.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 3.3. Primer: zahtev prolazi kroz tri komponente

Korisnik pokreće operaciju kroz API, API objavljuje poruku, a worker završava obradu. Ako svaka komponenta beleži svoj nepovezan identifikator, tim ne može brzo da odgovori da li je zahtev prihvaćen, obrađen ili izgubljen između koraka. Zajednički kontekst pretvara tri zasebna zapisa u jednu proverljivu priču.

- Korelacioni identifikator nastaje na granici toka i prenosi se dalje.
- Audit čuva odluke koje utiču na odgovornost, dok log čuva tehnički kontekst.
- Konfiguracija određuje očekivano ponašanje, kao što su rokovi i pragovi, i mora biti proverljiva.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 3.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 3.5. Šema sledljivog distribuiranog toka

### Šema sledljivog distribuiranog toka

- Ulazni zahtev: identitet i korelacija
- API: prihvata ili odbija
- Poruka i worker: obrađuju isti kontekst
- Audit i trag: objašnjavaju ishod

Jedan kontekst prolazi kroz sve komponente, pa se neuspeh može objasniti bez nagađanja.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 3.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Identitet | ko pokreće radnju | Šta se dešava kada identitet nije dostupan ili daje zastarelu informaciju? |
| Korelacija | jedan tok kroz komponente | Šta se dešava kada korelacija nije dostupan ili daje zastarelu informaciju? |
| Audit | poslovno značajna odluka | Šta se dešava kada audit nije dostupan ili daje zastarelu informaciju? |
| Konfiguracija | uslovi rada sistema | Šta se dešava kada konfiguracija nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 3.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Audit čuva odluke koje su važne za odgovornost i istragu.
2. Logovi i tragovi čuvaju tehnički kontekst, ali ne tajne.
3. Konfiguracija ima validno značenje, vlasnika i način promene.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Kako biste rekonstruisali jednu neuspelu operaciju koja je prošla kroz API, red poruka i worker, a da ne čitate neograničenu količinu logova?

### 3.8. Izvršivi primer

Primer prati jednu operaciju kroz dve komponente. Zakazivanje kontakta sa stanicom dobija identifikator operacije (`CorrelationId`) i identitet aktera; isti identifikator stiže do stanice i do revizijskog traga, pa se tok može rekonstruisati i kada delovi sistema vode odvojene logove. Ograničenje trajanja kontakta dolazi iz konfiguracije koja se proverava pre pokretanja.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija
dotnet run --project src/Odp.Vezba03.ConsoleUi
dotnet test Odp.Vezba03.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija/src/Odp.Vezba03.Application/Contacts/ScheduleContactHandler.cs

```csharp
public ScheduleContactResult Schedule(ContactRequest request, OperationContext context)
{
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(context);

    var code = Decide(request, context);

    auditLog.Record(new AuditEntry(
        clock.UtcNow,
        context.CorrelationId,
        context.ActorId,
        Action,
        request.StationId,
        code));

    return new ScheduleContactResult(code == ContactCodes.Scheduled, code, context.CorrelationId);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija/tests/Odp.Vezba03.Tests/Application/ScheduleContactHandlerTests.cs

```csharp
[Test]
public void Schedule_CarriesTheSameCorrelationIdToTheStationAndTheAuditTrail()
{
    var result = _handler.Schedule(Request, Context);

    using (Assert.EnterMultipleScope())
    {
        Assert.That(result, Is.EqualTo(new ScheduleContactResult(true, ContactCodes.Scheduled, "op-42")));
        Assert.That(_stationGateway.Calls.Single().CorrelationId, Is.EqualTo("op-42"));
        Assert.That(
            _auditLog.Entries.Single(),
            Is.EqualTo(new AuditEntry(
                Now,
                "op-42",
                "ana",
                "contact.schedule",
                "GS-NOVI-SAD",
                ContactCodes.Scheduled)));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba03.ConsoleUi

```text
op-0001: GS-NOVI-SAD, 10 min -> contact_scheduled
op-0002: GS-NOVI-SAD, 40 min -> duration_too_long
op-0003: GS-BEOGRAD, 10 min -> station_unavailable

Log stanice:
  op-0001 | GS-NOVI-SAD | reserved=True
  op-0003 | GS-BEOGRAD | reserved=False

Revizijski trag:
  op-0001 | ana | contact.schedule | GS-NOVI-SAD | contact_scheduled
  op-0002 | marko | contact.schedule | GS-NOVI-SAD | duration_too_long
  op-0003 | jelena | contact.schedule | GS-BEOGRAD | station_unavailable
```

#### Šta treba uočiti

- Isti `CorrelationId` vidi se u logu stanice i u revizijskom tragu centra.
- Revizijski zapis nastaje i kada je zahtev odbijen pre poziva stanice.
- Neispravna konfiguracija zaustavlja pokretanje sa kodom greške.

> **Zadatak uz primer**
>
> Dodati u `OperationContext` identifikator pozivajuće komponente (`Source`) i upisati ga u revizijski zapis. Proširiti test tako da dokazuje da zapis sadrži i izvor operacije.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Identitet, audit, observability i konfiguracija". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.
