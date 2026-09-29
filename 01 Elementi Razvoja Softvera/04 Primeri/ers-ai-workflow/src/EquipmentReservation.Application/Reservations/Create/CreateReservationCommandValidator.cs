namespace EquipmentReservation.Application.Reservations.Create;

public static class CreateReservationCommandValidator
{
    public static void ValidateAndThrow(CreateReservationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.RequestId == Guid.Empty)
            throw new ArgumentException("Request id is required.", nameof(command));
        if (command.EquipmentId == Guid.Empty)
            throw new ArgumentException("Equipment id is required.", nameof(command));
        if (command.StudentId == Guid.Empty)
            throw new ArgumentException("Student id is required.", nameof(command));
        if (command.Quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(command), "Quantity must be positive.");
    }
}
