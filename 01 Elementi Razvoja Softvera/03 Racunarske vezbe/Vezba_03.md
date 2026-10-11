# Elementi razvoja softvera — Praktikum

FTN · Nastavni materijali

## Vežba 3 — Poslovna logika i aplikacioni sloj

Nakon uspostavljanja arhitektonskih granica potrebno je rasporediti odgovornosti između modela, validatora i servisa slučaja upotrebe. Model predstavlja stanje, validator proverava ispravnost ulaznih vrednosti, a aplikacioni servis koordinira poslovna pravila i zavisnosti. Nijedan od ovih elemenata ne treba da zavisi od konkretnog korisničkog interfejsa, baze podataka ili transportnog protokola.

### Tok jednog slučaja upotrebe

- Ulaz: komanda ili zahtev
- Validacija: osnovna i poslovna pravila
- Poslovna pravila: odluka i promena stanja
- Čuvanje: ugovor repozitorijuma
- Rezultat: eksplicitan ishod za klijenta

Aplikacioni servis povezuje zahtev sa poslovnim pravilima i eksplicitno definisanim ishodom.

> **Studija slučaja: ECommerce**
>
> Primer ECommerce razdvaja Domain, podatke i repozitorijume, Application i implementaciju poslovnih servisa. Upiti i komande pripadaju aplikacionom sloju, dok konkretna baza podataka ili spoljni web servis ostaju infrastrukturni detalji.

> **Rešenje za preuzimanje**
>
> Kompletno rešenje studije slučaja dostupno je kao [ECommerce ZIP](https://owlcoder.github.io/ftn-nastavni-portal/E-Commerce.zip).

### 3.1. Model, validacija i poslovna pravila

Klasa modela predstavlja podatke i stanje rezervacije. Provera opsega i međusobnog odnosa ulaznih vrednosti pripada posebnom validatoru, dok pravila koja zahtevaju podatke iz repozitorijuma ili drugih modula pripadaju aplikacionom ili domenskom servisu. Ovakva podela sprečava da model istovremeno postane nosilac podataka, validator i servis.

Model čuva stanje, a validator proverava ispravnost perioda

```csharp
public sealed class Reservation
{
    public Guid Id { get; }
    public Guid EquipmentId { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public bool Cancelled { get; private set; }

    public Reservation(
        Guid id, Guid equipmentId, DateTime from, DateTime to)
    {
        Id = id;
        EquipmentId = equipmentId;
        From = from;
        To = to;
    }

    public void MarkCancelled() => Cancelled = true;
}

public static class ReservationValidator
{
    public static Result ValidatePeriod(DateTime from, DateTime to)
        => from < to
            ? Result.Ok()
            : Result.Fail("InvalidPeriod");
}
```

- Model predstavlja stanje i ne pristupa repozitorijumu, vremenu sistema niti spoljnim servisima.
- Validator proverava format, opseg i međusobni odnos ulaznih vrednosti.
- Aplikacioni ili domenski servis sprovodi pravila koja zahtevaju dodatni kontekst.
- Promena stanja modela izvodi se ograničenom operacijom nakon uspešne validacije i poslovne provere.

### 3.2. `Result` i očekivani neuspeh

Očekivani poslovni neuspeh nije isto što i neočekivana greška sistema. Kada korisnik pokuša da rezerviše zauzet termin, sistem radi ispravno ako zahtev odbije i vrati stabilan i razumljiv ishod. Takvo ponašanje treba da bude eksplicitno i testabilno, umesto da se prikrije vrednostima `null`, `false` ili generičkim izuzetkom.

Eksplicitan rezultat poslovne operacije u aplikacionom servisu

```csharp
public sealed record Result(bool Success, string? Error)
{
    public static Result Ok() => new(true, null);
    public static Result Fail(string error) => new(false, error);
}

public sealed class ReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
        => _repository = repository;

    public Result Reserve(ReservationRequest request)
    {
        var validation = ReservationValidator.ValidatePeriod(
            request.From, request.To);
        if (!validation.Success)
            return validation;

        if (_repository.HasOverlap(
            request.EquipmentId, request.From, request.To))
            return Result.Fail("OverlappingReservation");

        var reservation = new Reservation(
            Guid.NewGuid(), request.EquipmentId,
            request.From, request.To);

        _repository.Add(reservation);
        return Result.Ok();
    }
}
```

| Situacija | Preporučeni model |
| --- | --- |
| Nevažeći poslovni zahtev | Eksplicitan Result i stabilan kod poslovnog neuspeha. |
| Entitet nije pronađen | Result sa stabilnim kodom ili domenom definisan NotFound ishod. |
| Baza privremeno nedostupna | Infrastrukturna greška koja se obrađuje na granici sistema (middleware, retry politika). |
| Narušena pretpostavka unutar domena | Result.Fail sa jasnim kodom — poslovni sloj ostaje predvidljiv i bez izuzetaka. |

### 3.3. Servis slučaja upotrebe i dependency injection

Dependency Injection nije samo mogućnost framework-a. To je način da objekat dobije saradnike spolja, tako da centralna logika ne mora da zna kako se ti saradnici kreiraju. Time se promenljive zavisnosti mogu zameniti u testu ili drugom izvršnom okruženju.

Servis slučaja upotrebe sa ubrizganim zavisnostima

```csharp
public sealed class ReservationService
{
    private readonly IReservationRepository _repository;
    private readonly IClock _clock;

    public ReservationService(
        IReservationRepository repository,
        IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public Result Reserve(ReservationRequest request)
    {
        var validation = ReservationValidator.ValidatePeriod(
            request.From, request.To);
        if (!validation.Success)
            return validation;

        if (request.From <= _clock.UtcNow)
            return Result.Fail("ReservationMustBeInFuture");

        if (_repository.HasOverlap(
            request.EquipmentId, request.From, request.To))
            return Result.Fail("OverlappingReservation");

        var reservation = new Reservation(
            Guid.NewGuid(), request.EquipmentId,
            request.From, request.To);

        _repository.Add(reservation);
        return Result.Ok();
    }
}
```

> **Testabilnost se projektuje unapred**
>
> `IClock` i `IReservationRepository` nisu uvedeni samo radi testova, već zato što vreme i skladište predstavljaju promenljive spoljne zavisnosti. Testiranje koristi činjenicu da su granice sistema već jasno postavljene.

> **Mini domaći — bonus 1 bod**
>
> Izabrati metodu koja za očekivani poslovni neuspeh vraća `bool`, `null` ili generički `Exception`. Preoblikovati je u eksplicitan rezultat i dodati dva mala testa ili demonstraciona scenarija.
