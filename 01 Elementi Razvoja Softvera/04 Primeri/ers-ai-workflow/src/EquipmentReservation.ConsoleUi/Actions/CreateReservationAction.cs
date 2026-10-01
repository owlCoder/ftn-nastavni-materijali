using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.ConsoleUi.Menu;
using EquipmentReservation.ConsoleUi.Terminal;

namespace EquipmentReservation.ConsoleUi.Actions;

public sealed class CreateReservationAction : IMenuAction
{
    private readonly ICreateReservationUseCase _createReservation;
    private readonly IGetEquipmentAvailabilityUseCase _getAvailability;
    private readonly ITerminal _terminal;
    private readonly Guid _equipmentId;

    public CreateReservationAction(
        ICreateReservationUseCase createReservation,
        IGetEquipmentAvailabilityUseCase getAvailability,
        ITerminal terminal,
        Guid equipmentId)
    {
        _createReservation = createReservation
            ?? throw new ArgumentNullException(nameof(createReservation));
        _getAvailability = getAvailability ?? throw new ArgumentNullException(nameof(getAvailability));
        _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _equipmentId = equipmentId;
    }

    public string Key => "2";

    public string Label => "Kreiraj rezervaciju";

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!TryReadStudentId(out var studentId))
        {
            _terminal.WriteLine("Student ID nije validan GUID.");
            return;
        }

        if (!TryReadQuantity(out var quantity))
        {
            _terminal.WriteLine("Količina mora biti ceo broj.");
            return;
        }

        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            _equipmentId,
            studentId,
            quantity);

        var result = await _createReservation.HandleAsync(command, cancellationToken);

        _terminal.WriteLine($"Request ID: {command.RequestId}");
        _terminal.WriteLine($"Reservation ID: {result.ReservationId?.ToString() ?? "-"}");
        _terminal.WriteLine($"Status: {result.Outcome}");
        _terminal.WriteLine($"Error: {result.ErrorCode ?? "-"}");

        var availability = await _getAvailability.HandleAsync(
            new GetEquipmentAvailabilityQuery(_equipmentId),
            cancellationToken);
        _terminal.WriteLine(
            $"Preostalo: {(availability.Success ? availability.Value.Available.ToString() : "n/a")}");
    }

    private bool TryReadStudentId(out Guid studentId)
    {
        _terminal.Write("Student ID (Enter = generiši): ");
        var input = _terminal.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input))
        {
            studentId = Guid.NewGuid();
            return true;
        }

        return Guid.TryParse(input, out studentId);
    }

    private bool TryReadQuantity(out int quantity)
    {
        _terminal.Write("Količina: ");
        return int.TryParse(_terminal.ReadLine(), out quantity);
    }
}
