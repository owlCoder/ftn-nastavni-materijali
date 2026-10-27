namespace EquipmentReservation.Application.Reservations.Create;

public sealed class CreateReservationCommand
{
    public CreateReservationCommand() { }

    public CreateReservationCommand(Guid requestId, Guid equipmentId, Guid studentId, int quantity)
    {
        RequestId = requestId;
        EquipmentId = equipmentId;
        StudentId = studentId;
        Quantity = quantity;
    }

    public Guid RequestId { get; init; }
    public Guid EquipmentId { get; init; }
    public Guid StudentId { get; init; }
    public int Quantity { get; init; }
}
