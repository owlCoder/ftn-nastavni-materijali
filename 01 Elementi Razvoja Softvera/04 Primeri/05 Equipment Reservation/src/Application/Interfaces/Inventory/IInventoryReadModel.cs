using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.Application.Interfaces.Inventory;

public interface IInventoryReadModel
{
    Task<EquipmentAvailability> GetAvailableAsync(Guid equipmentId, CancellationToken cancellationToken);
}
