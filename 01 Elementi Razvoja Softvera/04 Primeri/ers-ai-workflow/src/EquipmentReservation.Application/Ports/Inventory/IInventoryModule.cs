using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Ports.Inventory;

public interface IInventoryModule
{
    Task<Result> ReserveAsync(
        ReserveInventoryRequest request,
        CancellationToken cancellationToken);
}
