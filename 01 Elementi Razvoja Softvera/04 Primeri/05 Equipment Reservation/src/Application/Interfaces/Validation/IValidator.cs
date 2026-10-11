using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Interfaces.Validation;

public interface IValidator<in T>
{
    Result Validate(T instance);
}
