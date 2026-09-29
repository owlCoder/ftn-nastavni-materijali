namespace EquipmentReservation.Application.Ports.Inventory;

public interface IInventoryReadModel
{
    Task<int?> GetAvailableAsync(
        Guid equipmentId,
        CancellationToken cancellationToken);
}
