using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Common;

public interface IValidator<in T>
{
    Result Validate(T instance);
}
