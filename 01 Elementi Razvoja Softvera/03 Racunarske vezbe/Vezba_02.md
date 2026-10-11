# Elementi razvoja softvera — Praktikum

FTN · Nastavni materijali

## Vežba 2 — SOLID i Clean Architecture

SOLID principi usmeravaju oblikovanje sistema u kome su promene lokalizovane, zavisnosti zamenjive, a ponašanje dostupno proveri. Njihova primena zasniva se na razumevanju ugovora, enkapsulacije, polimorfizma i odgovornosti. Clean Architecture isti pristup primenjuje na granice podsistema: poslovno jezgro ostaje nezavisno od korisničkog interfejsa, skladišta podataka i drugih promenljivih tehničkih detalja.

| Princip | Pitanje pri pregledu koda |
| --- | --- |
| S — Single Responsibility | Da li ovaj modul ima jedan koherentan razlog za promenu? |
| O — Open/Closed | Može li se nova varijanta ponašanja dodati bez izmene stabilnog centralnog toka? |
| L — Liskov Substitution | Može li jedna implementacija biti zamenjena drugom bez narušavanja očekivanja klijenta? |
| I — Interface Segregation | Da li klijent zavisi samo od operacija koje zaista koristi? |
| D — Dependency Inversion | Da li poslovna logika zavisi od apstrakcije ili neposredno od tehničkog detalja? |

![Clean Architecture](https://owlcoder.github.io/ftn-nastavni-portal/course-assets/clean-architecture.svg)

Clean Architecture: poslovno jezgro ostaje nezavisno od spoljašnjih tehnoloških detalja.

### 2.1. OOP i Clean Code kao osnova

Interfejs opisuje ugovor koji klijentski kod može da koristi bez poznavanja konkretne implementacije. Enkapsulacija čuva validno stanje objekta i usmerava promene kroz dozvoljene operacije. Polimorfizam omogućava da različite implementacije istog ugovora budu zamenjive bez grananja po konkretnom tipu. Apstraktna klasa ima smisla kada više tipova zaista deli zajedničko stanje ili ponašanje; nije potrebna samo zato što postoji hijerarhija.

| Pojam | Praktično značenje u dizajnu |
| --- | --- |
| Interfejs | Definiše očekivano ponašanje i granicu između klijenta i implementacije. |
| Enkapsulacija | Čuva invarijante i sprečava nekontrolisanu izmenu stanja. |
| Polimorfizam | Omogućava zamenu implementacije bez promene klijentskog koda. |
| Apstraktna klasa | Čuva stvarno zajedničku osnovu kada više tipova deli stanje ili ponašanje. |
| Clean Code | Čini nameru, odgovornost i tok promene lakšim za razumevanje i proveru. |

Klijent zavisi od ugovora, a ne od konkretne implementacije

```csharp
public interface IPretrazivac
{
    int Prebroj(string pojam, IReadOnlyList<string> vrednosti);
}

public sealed class LinearniPretrazivac : IPretrazivac
{
    public int Prebroj(string pojam, IReadOnlyList<string> vrednosti)
        => vrednosti.Count(v => v == pojam);
}

public static int PronadjiBrojPogodaka(
    IPretrazivac pretrazivac,
    string pojam,
    IReadOnlyList<string> vrednosti)
    => pretrazivac.Prebroj(pojam, vrednosti);
```

- Ugovor mora precizno da odredi značenje ulaza, izlaza i graničnih slučajeva.
- Zamenjive implementacije moraju da poštuju ista očekivanja klijenta.
- Nazivi klasa i metoda treba da opisuju nameru, a ne tehnički detalj bez poslovnog značenja.
- Metoda ili klasa koja istovremeno rešava više nepovezanih problema verovatno ima nejasnu odgovornost.
- Dupliranje se uklanja kada postoji zajednički koncept, a ne samo zato što dva dela koda trenutno izgledaju slično.

![Od nejasne odgovornosti ka jasnim granicama](https://owlcoder.github.io/ftn-nastavni-portal/course-assets/oop-refactor.svg)

Razdvajanje pristupa podacima, poslovnog pravila i evidentiranja aktivnosti iz preopterećenog servisa.

> **Veza sa SOLID principima**
>
> Ovi pojmovi nisu posebna tema odvojena od SOLID-a. SRP precizira odgovornost, OCP i LSP se oslanjaju na zamenjivo ponašanje, ISP na dobro oblikovane ugovore, a DIP na zavisnost od apstrakcija umesto od promenljivih tehničkih detalja.

### 2.2. Single Responsibility Principle

Posmatrajmo servis koji istovremeno generiše sadržaj poruke, čita korisnike i šalje e-mail. Problem nije broj linija koda, već činjenica da se ista klasa menja iz više međusobno nezavisnih razloga. SRP usmerava dizajn ka koherentnim komponentama čija se odgovornost i razlog za izmenu mogu jasno opisati.

Servis orkestrira saradnike umesto da preuzima njihove odgovornosti

```csharp
public sealed class NotificationService
{
    private readonly ISubscriberRepository _subscribers;
    private readonly IMessageComposer _composer;
    private readonly IMessageSender _sender;

    public NotificationService(
        ISubscriberRepository subscribers,
        IMessageComposer composer,
        IMessageSender sender)
    {
        _subscribers = subscribers;
        _composer = composer;
        _sender = sender;
    }

    public void NotifyAll(string eventName)
    {
        var message = _composer.Compose(eventName);
        foreach (var subscriber in _subscribers.GetActive())
            _sender.Send(subscriber.Contact, message);
    }
}
```

> **SRP ne znači „jedna metoda po klasi“**
>
> Klasa može imati više metoda kada sve pripadaju istoj odgovornosti. Neprimerena dekompozicija nastaje kada se jedna koherentna odgovornost nepotrebno raspodeli na veliki broj sitnih tipova bez jasne poslovne ili tehničke granice.

> **Rad na vežbi**
>
> U nastavnom ili projektnom kodu pronaći klasu koja obavlja najmanje dve nepovezane odgovornosti. Napisati jednu rečenicu koja opisuje njenu trenutnu ulogu, zatim predložiti razdvajanje i obrazložiti koje će buduće promene takvim dizajnom biti lokalizovane.

### 2.3. Open/Closed Principle

Primer sa slanjem obaveštenja pokazuje tipičan OCP problem: ako centralni servis proverava tip poruke i grananjem bira e-mail, SMS i svaku novu vrstu, svako proširenje zahteva izmenu stabilnog koda. Promenljivo ponašanje je prikladnije smestiti iza ugovora, tako da se nova implementacija dodaje bez izmene postojećeg klijenta.

Zamenjive implementacije istog ugovora

```csharp
public interface IMessageSender
{
    string Channel { get; }
    Task SendAsync(string destination, string message);
}

public sealed class EmailSender : IMessageSender
{
    public string Channel => "email";
    public Task SendAsync(string destination, string message)
        => Task.CompletedTask; // adapter ka realnom servisu
}

public sealed class SmsSender : IMessageSender
{
    public string Channel => "sms";
    public Task SendAsync(string destination, string message)
        => Task.CompletedTask;
}
```

- Nova vrsta kanala treba da se doda novom implementacijom, bez izmene poslovne logike slanja.
- Ako se u stabilnom delu sistema često pojavljuje `switch` po tipu koji određuje strategiju ponašanja, treba razmotriti da li je polimorfizam prikladniji.
- Apstrakciju ne treba uvoditi unapred za hipotetičke promene. Cilj je kontrolisati poznatu ili verovatnu varijabilnost, a ne maksimalno povećati broj interfejsa.

### 2.4. LSP, ISP i DIP

Liskov Substitution Principle zahteva da implementacija poštuje očekivanja ugovora. Sintaksno ispravno nasleđivanje nije dovoljno ako podtip menja značenje operacije ili narušava postuslove. Interface Segregation Principle ograničava širinu ugovora, dok Dependency Inversion Principle usmerava zavisnost od politike višeg nivoa ka apstrakcijama, a ne ka promenljivim tehničkim detaljima.

| Princip | Primer problema | Smer korekcije |
| --- | --- | --- |
| LSP | Podtip menja postuslov ili ne podržava operaciju roditelja. | Redizajnirati ugovor ili koristiti zajedničku apstrakciju bez neprirodnog nasleđivanja. |
| ISP | Običan korisnik mora da implementira 2FA operacije koje nikada ne koristi. | Podeliti veliki interfejs na manje uloge, na primer `ILogin` i `ITwoFactorAuth`. |
| DIP | Use-case neposredno kreira SQL repozitorijum ili HTTP klijent. | Use-case zavisi od ugovora; konkretna implementacija se bira u composition root-u. |

Poslovna politika koristi ugovor repozitorijuma, a ne njegovu konkretnu implementaciju

```csharp
public sealed class RegisterReservationHandler
{
    private readonly IReservationRepository _repository;
    private readonly IClock _clock;

    public RegisterReservationHandler(
        IReservationRepository repository,
        IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public Result Handle(RegisterReservation command)
    {
        if (command.From <= _clock.UtcNow)
            return Result.Fail("ReservationMustBeInFuture");

        if (_repository.HasOverlap(command.EquipmentId, command.From, command.To))
            return Result.Fail("OverlappingReservation");

        _repository.Add(new Reservation(
            Guid.NewGuid(), command.EquipmentId, command.From, command.To));

        return Result.Ok();
    }
}
```

### 2.5. Clean Architecture i smer zavisnosti

Clean Architecture uređuje granice i smer zavisnosti. Domain sadrži modele i domenske ugovore. Application realizuje slučajeve upotrebe, koordinira validatore i poslovne servise i definiše portove potrebne aplikaciji. Infrastructure implementira pristup bazi podataka, datotekama i spoljnim servisima, dok Presentation prevodi korisnički ili mrežni ulaz u pozive aplikacionog sloja. Nazivi projekata mogu se razlikovati, ali unutrašnji slojevi ne smeju zavisiti od promenljivih spoljnih detalja.

### Smer zavisnosti

- Presentation: UI, API ili konzola; bez poslovnih pravila
- Application: slučajevi upotrebe, validacija i poslovni servisi
- Domain: modeli i domenski ugovori
- Infrastructure: baza, datoteke i spoljni adapteri

Spoljni slojevi mogu da zavise od ugovora unutrašnjih slojeva; poslovno jezgro ne zavisi od framework-a i skladišta.

- Kontroler mapira zahtev, poziva slučaj upotrebe i prevodi rezultat u odgovor.
- Domain ne referencira Entity Framework, HTTP, konzolu niti UI framework.
- Composition root je mesto na kome se bira konkretan graf zavisnosti.
- Interfejs pripada sloju koji definiše potrebu ili ugovor, a ne automatski sloju koji ga implementira.

> **Broj projekata nije metrika kvaliteta arhitekture**
>
> Moguće je imati veliki broj projekata i nejasne granice, kao i jednostavniju fizičku strukturu sa ispravnim smerom zavisnosti. Ocenjuju se objašnjiv dizajn, odgovornosti slojeva i lokalizacija promena.

### 2.6. Studija slučaja: Logger–Blogger

Logger–Blogger služi kao studija slučaja za razdvajanje poslovnog jezgra, infrastrukturnih detalja i composition root-a. Njegova svrha nije da bude šablon koji se prepisuje u projekat, već da omogući poređenje odgovornosti slojeva i načina na koji Dependency Inversion štiti unutrašnji kod od promena skladišta, logovanja ili korisničkog interfejsa.

| Projekat | Sadržaj i odgovornost |
| --- | --- |
| Domain | Poslovni modeli `Korisnik`, `ZapisNaSajtu`, `Objava`, `Evidencija` i nabrajanja. Ne zavisi ni od jednog drugog projekta. |
| Application | Registracija, prijava i objave/evidencije; Result tip i ugovori za servise, repozitorijume, logger i hasher. Zavisi samo od Domain-a. |
| Infrastructure | PBKDF2 hasher sa salt-om i file logger. Implementira ugovore Application sloja. |
| Database | InMemory i JSON provider, factory, ugovori baze i model tabela. Repozitorijumi implementiraju ugovore Application sloja. |
| Presentation | Konzolni meni, `IConsoleIO`, formatter, seeder, `AppContainer` i `Program.cs`; sastavlja graf zavisnosti. |
| Tests | NUnit/Moq provere use-case-ova, arhitekture, JSON adaptera i PBKDF2 hashera. |

Application/Interfaces/Services/IZapisiServis.cs i Database/Interfaces/IBazaPodataka.cs

```csharp
public interface IZapisiServis
{
    Result<ZapisNaSajtu> Objavi(ZapisNaSajtu zapis);

    Result<IReadOnlyList<ZapisNaSajtu>> PregledZapisa();
}

public interface IBazaPodataka
{
    TabeleBazaPodataka Tabele { get; }
    bool IsReady { get; }
    bool SacuvajPromene();
}
```

> **Ključna ideja**
>
> Način čuvanja podataka i način logovanja mogu da se promene bez izmene centralnog poslovnog toka. Ista ideja se prenosi u studentski projekat, ali tim bira sopstvene granice i nazive u skladu sa domenom.

> **Rešenje za preuzimanje**
>
> Kompletno rešenje studije slučaja dostupno je kao [Logger–Blogger ZIP](https://owlcoder.github.io/ftn-nastavni-portal/Logger-Bloger.zip). Komande za pokretanje nalaze se u [korenskom README dokumentu nastavnih materijala](https://github.com/owlCoder/ftn-nastavni-materijali#programski-primeri).

### 2.7. Composition root

Composition root je mesto na kome se konkretne implementacije povezuju u izvršivi graf objekata. Poslovni kod ne treba sam da bira `JsonRepository` ili `SqlRepository`; taj izbor pripada spoljašnjoj konfiguraciji aplikacije.

Jedno mesto bira konkretne implementacije

```csharp
var repository = new JsonReservationRepository("data/reservations.json");
var clock = new SystemClock();
var service = new ReservationService(repository, clock);
var app = new ConsoleApplication(service);

app.Run();
```

> **Rad na vežbi**
>
> Kreirati solution i početne projekte ili slojeve bez unapred pripremljenog projektnog šablona, povezati ih u jednom composition root-u i u `docs/architecture.md` zapisati najmanje dve arhitektonske odluke sa obrazloženjem izbora.

> **Mini domaći — bonus 2 boda**
>
> Dodati alternativnu implementaciju jedne spoljne zavisnosti, na primer InMemory umesto JSON repozitorijuma. Prvi bod se dobija ako use-case kod ostane neizmenjen; drugi bod za jasno obrazloženje kako DIP/OCP omogućavaju zamenu.

### Kontrolna tačka P1 — Problem, backlog i razvojni tok

U ovoj fazi ne zahteva se završena arhitektura niti veliki obim implementacije. Tim obrazlaže problem, razlaže rad na proverljive stavke i dokumentuje početni tok zajedničkog razvoja u repozitorijumu.

- Zajednički repozitorijum sa README dokumentom i pristupom svih članova tima.
- Tapiz Boards sadrži backlog sa prioritetima i proverljivim kriterijumima prihvatanja.
- Najmanje jedan zahtev za spajanje (pull request) sadrži pregled izmena i obrazložene komentare učesnika.
- Tim koristi dogovoreni tok Backlog → Ready → In Progress → Code Review → QA/Verify → Done.

> **Termin i obuhvat**
>
> Vežba 1–2 · nedelja od 19.10. Isti zahtevi prikazani su na kartici „Kontrolne tačke“.
