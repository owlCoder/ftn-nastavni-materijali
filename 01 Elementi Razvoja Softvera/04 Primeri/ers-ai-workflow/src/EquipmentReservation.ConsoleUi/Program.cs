using System.Text;
using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;

Console.OutputEncoding = Encoding.UTF8;

var equipmentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
var inventory = new InMemoryInventoryModule();
inventory.Seed(equipmentId, available: 10);

var repository = new InMemoryReservationRepository();
var handler = new CreateReservationHandler(
    repository,
    inventory,
    new InMemoryReservationRequestLock());
IInventoryReadModel inventoryReadModel = inventory;

Console.WriteLine("Equipment Reservation — Console UI");
Console.WriteLine($"Demo equipment ID: {equipmentId}");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Prikaži stanje opreme");
    Console.WriteLine("2 - Kreiraj rezervaciju");
    Console.WriteLine("0 - Izlaz");
    Console.Write("Izbor: ");

    var choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            await ShowAvailabilityAsync(inventoryReadModel, equipmentId);
            break;
        case "2":
            await CreateReservationAsync(handler, inventoryReadModel, equipmentId);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Nepoznata opcija.");
            break;
    }
}

static async Task ShowAvailabilityAsync(IInventoryReadModel inventory, Guid equipmentId)
{
    var available = await inventory.GetAvailableAsync(equipmentId, CancellationToken.None);
    Console.WriteLine(available is null
        ? "Oprema nije pronađena."
        : $"Dostupno komada: {available}");
}

static async Task CreateReservationAsync(
    CreateReservationHandler handler,
    IInventoryReadModel inventory,
    Guid equipmentId)
{
    Console.Write("Student ID (Enter = generiši): ");
    var studentInput = Console.ReadLine()?.Trim();
    var studentId = string.IsNullOrWhiteSpace(studentInput)
        ? Guid.NewGuid()
        : Guid.TryParse(studentInput, out var parsedStudentId)
            ? parsedStudentId
            : Guid.Empty;

    if (studentId == Guid.Empty)
    {
        Console.WriteLine("Student ID nije validan GUID.");
        return;
    }

    Console.Write("Količina: ");
    if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
    {
        Console.WriteLine("Količina mora biti pozitivan ceo broj.");
        return;
    }

    var requestId = Guid.NewGuid();
    var command = new CreateReservationCommand(
        requestId,
        equipmentId,
        studentId,
        quantity);

    var result = await handler.HandleAsync(command, CancellationToken.None);

    Console.WriteLine($"Request ID: {requestId}");
    Console.WriteLine($"Reservation ID: {result.ReservationId}");
    Console.WriteLine($"Status: {result.Outcome}");
    Console.WriteLine($"Error: {result.ErrorCode ?? "-"}");

    var available = await inventory.GetAvailableAsync(equipmentId, CancellationToken.None);
    Console.WriteLine($"Preostalo: {available?.ToString() ?? "n/a"}");
}
