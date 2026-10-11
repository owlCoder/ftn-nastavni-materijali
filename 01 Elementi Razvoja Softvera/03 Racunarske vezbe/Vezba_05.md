# Elementi razvoja softvera — Praktikum

FTN · Nastavni materijali

## Vežba 5 — Integracija modula, ugovori i podaci

Od ove vežbe do kraja praktikuma koristi se isti izvršivi primer: **Equipment Reservation**. Izvorni kod nalazi se u direktorijumu `examples/ers-ai-workflow/`, a rešenje se otvara datotekom `EquipmentReservation.sln`. Na istom sistemu redom se obrađuju integracija modula, razvoj uz podršku AI alata, MCP i izvršivi zaštitni mehanizmi.

Otvaranje i provera kompletnog nastavnog primera

```bash
cd examples/ers-ai-workflow
dotnet restore EquipmentReservation.sln
dotnet build EquipmentReservation.sln --configuration Release
dotnet test EquipmentReservation.sln --configuration Release --no-build
```

### Jedan solution, jasne granice

- Domain: modeli i domenski servisi
- Application: use-case i portovi
- Infrastructure: adapteri ka spoljnim detaljima
- API / Console UI: composition root i transport
- Tests: nezavisna provera ponašanja

MCP i Guardrails projekti postoje u istom solution-u, ali ne postaju zavisnosti poslovnog jezgra.

### 5.1. Struktura solution-a i Dependency Rule

Rešenje sadrži osam projekata. Prvih pet čine aplikaciju sa dva presentation adaptera, MCP i Guardrails pripadaju razvojnim alatima, dok projekat sa testovima proverava poslovno jezgro, izvršive zaštitne politike i sam smer zavisnosti. Smer zavisnosti ostaje osnovno arhitektonsko pravilo: unutrašnji slojevi ne poznaju spoljne detalje.

| Projekat | Odgovornost |
| --- | --- |
| Domain | Modeli, Result i domenski servisi sa poslovnim pravilima; nema projektnih zavisnosti. |
| Application | Slučajevi upotrebe, validatori i portovi prema drugim modulima/infrastrukturi. |
| Infrastructure | Implementacije portova; u primeru in-memory adapteri bez poslovnih odluka. |
| Api | Composition root i HTTP granica; ne sadrži poslovna pravila. |
| ConsoleUi | Drugi presentation adapter nad istim slučajevima upotrebe. |
| Mcp | Kontrolisano izlaganje projektnog konteksta AI klijentu. |
| Guardrails | Determinističke politike za rizične pozive alata. |
| Tests | NUnit provere domena, use-case-a, guardrail-a, AI artefakata i smera zavisnosti. |

> **Zašto je ovo Clean Architecture**
>
> Promena baze, AI klijenta, MCP transporta ili hook konfiguracije ne zahteva promenu poslovnih pravila. Spoljni detalji zavise ka unutra, a ne obrnuto. Test `DependencyRuleTests` pada ako neki projekat dobije zavisnost u pogrešnom smeru.

### 5.2. Poslovno pravilo ostaje u Domain sloju

Modeli `Reservation` i `InventoryItem` predstavljaju stanje i ne znaju za HTTP, bazu, MCP niti AI. Provera ulaznih vrednosti pripada validatoru, a pravilo raspoložive količine sprovodi domenski servis `InventoryReservationService`.

examples/ers-ai-workflow/src/Domain/Inventory/InventoryReservationService.cs

```csharp
public sealed class InventoryItem
{
    public InventoryItem() { }

    public InventoryItem(Guid equipmentId, int available)
    {
        EquipmentId = equipmentId;
        Available = available;
    }

    public Guid EquipmentId { get; init; }
    public int Available { get; init; }
}

public sealed class InventoryReservationService
{
    public Result<InventoryItem> Reserve(InventoryItem item, int quantity)
    {
        if (item is null || item.EquipmentId == Guid.Empty || item.Available < 0)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InvalidEquipment, new InventoryItem());

        if (quantity <= 0)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InvalidQuantity, new InventoryItem());

        if (item.Available < quantity)
            return Result<InventoryItem>.Fail(InventoryErrorCodes.InsufficientStock, new InventoryItem());

        return Result<InventoryItem>.Ok(new InventoryItem(item.EquipmentId, item.Available - quantity));
    }
}
```

- SRP: model čuva stanje; validator proverava ulaz; domenski servis sprovodi pravilo; use-case orkestrira; adapter čuva podatke.
- Kod neuspeha je deo poslovnog ishoda, a ne izuzetak infrastrukture.
- Isto pravilo koriste API, Console UI, testovi i budući adapteri, bez dupliranja.

### 5.3. Ugovor između Reservations i Inventory dela sistema

Application sloj definiše ono što use-case za rezervaciju zaista treba. Ne prosleđuje ORM entitet niti omogućava pristup internom skladištu Inventory modula. Ovo je praktična primena ISP i DIP.

examples/ers-ai-workflow/src/Application/Interfaces/

```csharp
public sealed class ReserveInventoryRequest
{
    public ReserveInventoryRequest() { }

    public ReserveInventoryRequest(Guid equipmentId, int quantity, Guid reservationId)
    {
        EquipmentId = equipmentId;
        Quantity = quantity;
        ReservationId = reservationId;
    }

    public Guid EquipmentId { get; init; }
    public int Quantity { get; init; }
    public Guid ReservationId { get; init; }
}

public interface IInventoryModule
{
    Task<Result> ReserveAsync(ReserveInventoryRequest request, CancellationToken cancellationToken);
    Task<Result> ReleaseAsync(ReserveInventoryRequest request, CancellationToken cancellationToken);
}

public interface IReservationRepository
{
    Task<Reservation> FindByRequestIdAsync(Guid requestId, CancellationToken cancellationToken);
    Task<bool> AddAsync(Reservation reservation, CancellationToken cancellationToken);
}
```

> **Dependency inversion**
>
> Use-case zavisi od ugovora koje poseduje Application sloj. Infrastructure bira kako će ti ugovori biti realizovani, a pravilo zalihe poziva iz domenskog servisa umesto da ga sam donosi.

### 5.4. CreateReservationHandler kao Application use-case

Handler proverava ulaz i otkazivanje zahteva, zaključava RequestId, proverava idempotentnost, poziva Inventory kroz port i čuva rezultat kroz repository port. Isti ključ sa drugačijim podacima vraća RequestPayloadMismatch. Posle neuspešnog upisa pokušava se kompenzacija zalihe, a njen neuspeh vraća InventoryCompensationFailed. Ne zna koja konkretna klasa čuva podatke i ne menja zalihu direktnim pristupom drugom modulu. API i Console UI ga pozivaju preko interfejsa `ICreateReservationUseCase`.

examples/ers-ai-workflow/src/Application/Reservations/Create/CreateReservationHandler.cs

```csharp
public async Task<CreateReservationResult> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken)
{
    var validation = _validator.Validate(command);
    if (!validation.Success)
        return CreateReservationResult.Invalid(validation.Error);
    if (cancellationToken.IsCancellationRequested)
        return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

    var acquisition = await _requestLock.AcquireAsync(command.RequestId, cancellationToken);
    if (!acquisition.Acquired)
        return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

    await using var lease = acquisition.Lease;
    if (cancellationToken.IsCancellationRequested)
        return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestCancelled);

    var existing = await _reservations.FindByRequestIdAsync(command.RequestId, cancellationToken);
    if (existing.Id != Guid.Empty)
    {
        if (existing.EquipmentId != command.EquipmentId || existing.StudentId != command.StudentId || existing.Quantity != command.Quantity)
            return CreateReservationResult.Invalid(CreateReservationErrorCodes.RequestPayloadMismatch);

        return CreateReservationResult.From(existing, replayed: true);
    }

    var reservationId = _idGenerator.NewReservationId();
    if (reservationId == Guid.Empty)
        return CreateReservationResult.Invalid(CreateReservationErrorCodes.PersistenceFailed);

    var inventoryRequest = new ReserveInventoryRequest(command.EquipmentId, command.Quantity, reservationId);
    var stock = await _inventory.ReserveAsync(inventoryRequest, cancellationToken);
    var reservation = stock.Success
        ? Reservation.Confirmed(reservationId, command.RequestId, command.EquipmentId, command.StudentId, command.Quantity)
        : Reservation.Rejected(reservationId, command.RequestId, command.EquipmentId, command.StudentId, command.Quantity, stock.Error);

    // After stock changes, complete persistence even if the caller disconnects.
    var saved = await _reservations.AddAsync(reservation, CancellationToken.None);
    if (!saved)
    {
        if (stock.Success)
        {
            var rollback = await _inventory.ReleaseAsync(inventoryRequest, CancellationToken.None);
            if (!rollback.Success)
                return CreateReservationResult.Invalid(CreateReservationErrorCodes.CompensationFailed);
        }

        return CreateReservationResult.Invalid(CreateReservationErrorCodes.PersistenceFailed);
    }

    return CreateReservationResult.From(reservation, replayed: false);
}
```

| Ishod | Značenje | HTTP odgovor |
| --- | --- | --- |
| Confirmed | Zaliha je umanjena i rezervacija sačuvana. | 200 |
| Rejected | Inventory je odbio zahtev, na primer `InsufficientStock`. | 409 |
| Invalid | Komanda nije prošla validaciju, na primer `QuantityMustBePositive`. | 400 |

### 5.5. Idempotentnost mora biti proverena testom

`RequestId` predstavlja idempotency key. Ako isti zahtev stigne ponovo, postojeća rezervacija se vraća bez drugog umanjenja zalihe. To nije komentar niti pretpostavka; ponašanje je zaključano testom.

examples/ers-ai-workflow/tests/Tests/Integration/ReservationFlowTests.cs

```csharp
[Test]
public async Task CreateReservation_WhenRequestIsRepeated_IsIdempotent()
{
    ComposeSystem(available: 5);
    var command = Command(quantity: 2);

    var first = await _handler.HandleAsync(command, CancellationToken.None);
    var replay = await _handler.HandleAsync(command, CancellationToken.None);

    using (Assert.EnterMultipleScope())
    {
        Assert.That(first.Outcome, Is.EqualTo(CreateReservationOutcome.Confirmed));
        Assert.That(replay.ReservationId, Is.EqualTo(first.ReservationId));
        Assert.That(replay.Replayed, Is.True);
        Assert.That(await AvailableAsync(), Is.EqualTo(3));
    }
}
```

Pored integracionih testova nad in-memory adapterima, `CreateReservationHandlerTests` proverava handler u izolaciji. Moq zamenjuje samo portove: repository, Inventory, lock i generator identifikatora.

> **Provera integracije**
>
> Ispravnost integracije potvrđuje se pokretanjem komande `dotnet test EquipmentReservation.sln` i pregledom rezultata testova.

### 5.6. SOLID mapa na stvarnom primeru

| Princip | Gde se vidi |
| --- | --- |
| SRP | Reservation čuva stanje; validator proverava ulaz; InventoryReservationService sprovodi pravilo zalihe; handler orkestrira use-case; adapter čuva podatke. |
| OCP | Nova Infrastructure implementacija ili nova opcija menija (IMenuAction) dodaje se bez menjanja handler-a. |
| LSP | Svaka IInventoryModule implementacija mora poštovati isti ugovor uspeha/neuspeha. |
| ISP | IInventoryModule izlaže rezervaciju i kompenzaciju zalihe (ReserveAsync i ReleaseAsync); čitanje stanja ide kroz poseban IInventoryReadModel. |
| DIP | API i Console UI zavise od ICreateReservationUseCase, handler od portova; composition root bira konkretne adaptere. |

> **Rad na vežbi**
>
> Otvoriti `EquipmentReservation.sln`, pronaći smer svih ProjectReference zavisnosti i nacrtati ga. Zatim zameniti jedan in-memory adapter sopstvenim test-double-om bez promene Domain/Application koda i pokrenuti ceo solution test.
