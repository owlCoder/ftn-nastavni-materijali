using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Inventory.GetAvailability;

public interface IGetEquipmentAvailabilityUseCase
{
    Task<Result<EquipmentAvailability>> HandleAsync(
        GetEquipmentAvailabilityQuery query,
        CancellationToken cancellationToken);
}
