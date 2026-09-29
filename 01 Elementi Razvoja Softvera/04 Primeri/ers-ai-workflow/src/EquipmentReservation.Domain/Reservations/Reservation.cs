namespace EquipmentReservation.Domain.Reservations;

public sealed class Reservation
{
    public Reservation(
        Guid id,
        Guid requestId,
        Guid equipmentId,
        Guid studentId,
        int quantity,
        ReservationStatus status,
        string? rejectionReason)
    {
        Id = id;
        RequestId = requestId;
        EquipmentId = equipmentId;
        StudentId = studentId;
        Quantity = quantity;
        Status = status;
        RejectionReason = rejectionReason;
    }

    public Guid Id { get; }
    public Guid RequestId { get; }
    public Guid EquipmentId { get; }
    public Guid StudentId { get; }
    public int Quantity { get; }
    public ReservationStatus Status { get; }
    public string? RejectionReason { get; }
}
