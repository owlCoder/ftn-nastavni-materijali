using EquipmentReservation.Application.Inventory.Models;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Interfaces.Inventory;

public interface IInventoryModule
{
    Task<Result> ReserveAsync(ReserveInventoryRequest request, CancellationToken cancellationToken);
    Task<Result> ReleaseAsync(ReserveInventoryRequest request, CancellationToken cancellationToken);
}
