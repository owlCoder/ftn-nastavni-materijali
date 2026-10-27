namespace EquipmentReservation.Api.Dtos;

public sealed class CreateReservationRequestDto
{
    public Guid RequestId { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid StudentId { get; set; }
    public int Quantity { get; set; }
}
