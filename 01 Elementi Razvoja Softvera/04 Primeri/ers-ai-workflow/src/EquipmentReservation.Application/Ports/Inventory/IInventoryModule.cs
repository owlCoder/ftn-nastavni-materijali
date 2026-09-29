namespace EquipmentReservation.Application.Ports.Inventory;

public interface IInventoryModule
{
    Task<ReserveInventoryResult> ReserveAsync(
        ReserveInventoryRequest request,
        CancellationToken cancellationToken);
}
