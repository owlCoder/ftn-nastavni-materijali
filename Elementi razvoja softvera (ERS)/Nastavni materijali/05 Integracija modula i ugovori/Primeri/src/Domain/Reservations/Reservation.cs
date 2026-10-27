namespace EquipmentReservation.Domain.Reservations;

public sealed class Reservation
{
    public Reservation() { }

    private Reservation(Guid id, Guid requestId, Guid equipmentId, Guid studentId, int quantity, ReservationStatus status, string rejectionReason)
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
    public ReservationStatus Status { get; } = ReservationStatus.Unknown;
    public string RejectionReason { get; } = string.Empty;

    public static Reservation Confirmed(Guid id, Guid requestId, Guid equipmentId, Guid studentId, int quantity)
    {
        return IsValid(id, requestId, equipmentId, studentId, quantity)
            ? new Reservation(id, requestId, equipmentId, studentId, quantity, ReservationStatus.Confirmed, string.Empty)
            : new Reservation();
    }

    public static Reservation Rejected(Guid id, Guid requestId, Guid equipmentId, Guid studentId, int quantity, string rejectionReason)
    {
        return IsValid(id, requestId, equipmentId, studentId, quantity) && !string.IsNullOrWhiteSpace(rejectionReason)
            ? new Reservation(id, requestId, equipmentId, studentId, quantity, ReservationStatus.Rejected, rejectionReason)
            : new Reservation();
    }

    private static bool IsValid(Guid id, Guid requestId, Guid equipmentId, Guid studentId, int quantity) =>
        id != Guid.Empty && requestId != Guid.Empty && equipmentId != Guid.Empty && studentId != Guid.Empty && quantity > 0;
}
