namespace EquipmentReservation.Api.Contracts;

public sealed record CreateReservationRequest(
    Guid RequestId,
    Guid EquipmentId,
    Guid StudentId,
    int Quantity);
