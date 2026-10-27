using EquipmentReservation.Application.Interfaces.Inventory;
using EquipmentReservation.Application.Interfaces.UseCases;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Inventory.GetAvailability;

public sealed class GetEquipmentAvailabilityHandler : IGetEquipmentAvailabilityUseCase
{
    private readonly IInventoryReadModel _inventory;

    public GetEquipmentAvailabilityHandler(IInventoryReadModel inventory)
    {
        _inventory = inventory;
    }

    public async Task<Result<EquipmentAvailability>> HandleAsync(GetEquipmentAvailabilityQuery query, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Result<EquipmentAvailability>.Fail(InventoryErrorCodes.RequestCancelled, new EquipmentAvailability());
        if (query is null || query.EquipmentId == Guid.Empty)
            return Result<EquipmentAvailability>.Fail(InventoryErrorCodes.InvalidEquipment, new EquipmentAvailability());

        var availability = await _inventory.GetAvailableAsync(query.EquipmentId, cancellationToken);
        return availability.EquipmentId == Guid.Empty
            ? Result<EquipmentAvailability>.Fail(InventoryErrorCodes.EquipmentNotFound, new EquipmentAvailability())
            : Result<EquipmentAvailability>.Ok(availability);
    }
}
