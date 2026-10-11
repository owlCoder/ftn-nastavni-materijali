# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## Vežba 6 — Komande, poslovi, retry i idempotentnost

Komanda je zahtev za promenu sveta, a ne običan poziv metode. Nakon slanja, sistem može čekati, ponavljati pokušaj ili dobiti kasnu potvrdu. Zato se životni ciklus komande modeluje eksplicitno i razlikuje od lokalne namere korisnika.

Retry povećava verovatnoću uspeha, ali bez idempotentnosti može ponoviti poslovni efekat. Lease i ograničeno vlasništvo nad poslom sprečavaju da dva workera istovremeno dovrše istu radnju nakon neusaglašenog pogleda na stanje.

### Kako sistem napreduje kada potvrda nije trenutna ni sigurna

- Zahtev: validirana namera
- Izvršenje: rad u pozadini
- Potvrda: može kasniti
- Ishod: jednom zabeležen rezultat

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 6.1. Ponovni pokušaj ne sme menjati značenje radnje

- Status komande opisuje znanje sistema, ne nagađanje o udaljenom svetu.
- Idempotency ključ vezuje ponovljene pokušaje za isti poslovni rezultat.
- Lease ima rok i štiti sistem od zakašnjelog rada prethodnog vlasnika.

> **Pitanje za diskusiju**
>
> Koja bi posledica duplog izvršenja bila neprihvatljiva i koji identitet operacije sprečava takav ishod?

### 6.2. Kako se ova tema vidi u razvoju

Tim razdvaja prijem zahteva, obradu u pozadini i prikaz ishoda. Zatim testira prekid između njih: ponovljeni zahtev, pad workera i kasna potvrda ne smeju proizvesti novu poslovnu radnju niti poništiti novije stanje.

#### Česte greške koje treba prepoznati

- Retry bez granice i bez razloga za prestanak.
- Dve obrade koje obe veruju da su vlasnici posla.
- Kasna potvrda koja prepisuje završeno stanje.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 6.3. Primer: ponovljen zahtev za istu radnju

Klijent ne dobija odgovor na vreme i šalje isti zahtev ponovo. Sistem želi da bude otporan na prekid, pa drugi zahtev ne sme automatski smatrati novom namerom. Idempotency ključ povezuje ponovljene pokušaje sa istim poslovnim rezultatom, dok status životnog ciklusa jasno odvaja prihvatanje, obradu, čekanje i završetak.

- Ponovni pokušaj koristi isti identitet operacije kada namera korisnika nije promenjena.
- Worker koji preuzima posao ima ograničeno vreme vlasništva i ne može završiti rad nakon isteka.
- Kasna potvrda ne sme prepisati novije, završeno stanje.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 6.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 6.5. Šema pouzdane komande

### Šema pouzdane komande

- Zahtev: jedinstvena namera
- Red ili posao: obrada u pozadini
- Lease: privremeno vlasništvo workera
- Ishod: jednom zabeležen rezultat

Retry povećava šansu da se radnja završi, dok idempotentnost čuva njeno značenje.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 6.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Zahtev | validirana namera | Šta se dešava kada zahtev nije dostupan ili daje zastarelu informaciju? |
| Izvršenje | rad u pozadini | Šta se dešava kada izvršenje nije dostupan ili daje zastarelu informaciju? |
| Potvrda | može kasniti | Šta se dešava kada potvrda nije dostupan ili daje zastarelu informaciju? |
| Ishod | jednom zabeležen rezultat | Šta se dešava kada ishod nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 6.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Status komande opisuje znanje sistema, ne nagađanje o udaljenom svetu.
2. Idempotency ključ vezuje ponovljene pokušaje za isti poslovni rezultat.
3. Lease ima rok i štiti sistem od zakašnjelog rada prethodnog vlasnika.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koja bi posledica duplog izvršenja bila neprihvatljiva i koji identitet operacije sprečava takav ishod?

### 6.8. Izvršivi primer

Primer vodi komandu ka udaljenom uređaju kroz neizvesnost. Slanje nema povratnu vrednost: potvrda stiže odvojeno, kasni ili ne stigne. Zato komanda ima stabilan identifikator, ponovno slanje nosi isti identifikator, uređaj ga izvršava jednom, a potvrda koja stigne posle isteka i dalje ispravlja stanje.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-06-komande-idempotentnost
dotnet run --project src/Odp.Vezba06.ConsoleUi
dotnet test Odp.Vezba06.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-06-komande-idempotentnost/src/Odp.Vezba06.Domain/Commands/CommandLifecycle.cs

```csharp
public (DeviceCommand Command, string Code) Acknowledge(DeviceCommand command) =>
    command.State switch
    {
        CommandState.Sent =>
            (command with { State = CommandState.Acknowledged }, CommandCodes.Acknowledged),
        CommandState.TimedOut =>
            (command with { State = CommandState.Acknowledged }, CommandCodes.LateAckReconciled),
        _ => (command, CommandCodes.AckDuplicate)
    };
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-06-komande-idempotentnost/tests/Odp.Vezba06.Tests/Application/CommandFlowTests.cs

```csharp
[Test]
public void Run_WhenAckIsMissing_ResendsTheSameCommandAndTheDeviceExecutesItOnce()
{
    _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
    _clock.Advance(TimeSpan.FromSeconds(12));

    var outcomes = _retry.Run();

    using (Assert.EnterMultipleScope())
    {
        Assert.That(outcomes, Is.EqualTo(new[] { new RetryOutcome("cmd-1", CommandCodes.Retried) }));
        Assert.That(_device.Deliveries, Is.EqualTo(2));
        Assert.That(_device.ExecutedCommandIds, Is.EqualTo(new[] { "cmd-1" }));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba06.ConsoleUi

```text
operator šalje komandu -> command_sent (pokušaj 1)
operator ponavlja isti zahtev -> command_already_dispatched (pokušaj 1)
potvrda nije stigla 12 s -> cmd-7: command_retried
potvrda nije stigla još 12 s -> cmd-7: attempts_exhausted
zakasnela potvrda -> late_ack_reconciled
ista potvrda ponovo -> ack_duplicate

Uređaj je primio 2 isporuke, a izvršio 1 komandu.
```

#### Šta treba uočiti

- Ponovljeni zahtev sa istim `CommandId` ne šalje komandu drugi put.
- Posle isteka se šalje ista komanda; uređaj je prima dva puta, a izvršava jednom.
- Potvrda koja stigne posle odustajanja ispravlja stanje (`late_ack_reconciled`).

> **Zadatak uz primer**
>
> Dodati odlaganje između pokušaja (`RetryDelay`) koje raste sa brojem pokušaja. Pravilo dodati u `CommandLifecycle` i testom pokazati da se ponovno slanje ne dešava pre isteka odlaganja.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Komande, poslovi, retry i idempotentnost". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P3 — Testiranje i manual-core-baseline

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.
