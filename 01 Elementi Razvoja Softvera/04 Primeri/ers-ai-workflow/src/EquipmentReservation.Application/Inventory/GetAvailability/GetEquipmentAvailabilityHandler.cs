using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Inventory.GetAvailability;

public sealed class GetEquipmentAvailabilityHandler : IGetEquipmentAvailabilityUseCase
{
    private readonly IInventoryReadModel _inventory;

    public GetEquipmentAvailabilityHandler(IInventoryReadModel inventory)
    {
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
    }

    public async Task<Result<EquipmentAvailability>> HandleAsync(
        GetEquipmentAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var available = await _inventory.GetAvailableAsync(query.EquipmentId, cancellationToken);

        return available is null
            ? Result<EquipmentAvailability>.Fail(InventoryErrorCodes.EquipmentNotFound)
            : Result<EquipmentAvailability>.Ok(
                new EquipmentAvailability(query.EquipmentId, available.Value));
    }
}
