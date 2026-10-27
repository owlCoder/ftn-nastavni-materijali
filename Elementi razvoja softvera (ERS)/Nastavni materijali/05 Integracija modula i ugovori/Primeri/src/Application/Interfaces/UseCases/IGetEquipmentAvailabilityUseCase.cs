using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Interfaces.UseCases;

public interface IGetEquipmentAvailabilityUseCase
{
    Task<Result<EquipmentAvailability>> HandleAsync(GetEquipmentAvailabilityQuery query, CancellationToken cancellationToken);
}
