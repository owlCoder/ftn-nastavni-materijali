using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Interfaces.UseCases;
using EquipmentReservation.ConsoleUi.Interfaces;
using EquipmentReservation.ConsoleUi.Terminal;
using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.ConsoleUi.Actions;

public sealed class ShowAvailabilityAction : IMenuAction
{
    private readonly IGetEquipmentAvailabilityUseCase _getAvailability;
    private readonly ITerminal _terminal;
    private readonly Guid _equipmentId;

    public ShowAvailabilityAction(IGetEquipmentAvailabilityUseCase getAvailability, ITerminal terminal, Guid equipmentId)
    {
        _getAvailability = getAvailability;
        _terminal = terminal ?? new SystemTerminal();
        _equipmentId = equipmentId;
    }

    public string Key => "1";

    public string Label => "Prikaži stanje opreme";

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await _getAvailability.HandleAsync(new GetEquipmentAvailabilityQuery(_equipmentId), cancellationToken);

        _terminal.WriteLine(result.Success
            ? $"Dostupno komada: {result.Value.Available}"
            : $"Oprema nije pronađena ({result.Error}).");
    }
}
