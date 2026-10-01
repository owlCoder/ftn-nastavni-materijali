using EquipmentReservation.Application.Common;
using EquipmentReservation.Domain.Shared;

namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationCommandValidator : IValidator<CreateReservationCommand>
{
    public Result Validate(CreateReservationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.RequestId == Guid.Empty)
            return Result.Fail(CreateReservationErrorCodes.RequestIdRequired);
        if (command.EquipmentId == Guid.Empty)
            return Result.Fail(CreateReservationErrorCodes.EquipmentIdRequired);
        if (command.StudentId == Guid.Empty)
            return Result.Fail(CreateReservationErrorCodes.StudentIdRequired);
        if (command.Quantity <= 0)
            return Result.Fail(CreateReservationErrorCodes.QuantityMustBePositive);

        return Result.Ok();
    }
}
