namespace EquipmentReservation.Api.Dtos;

public sealed class CreateReservationResponseDto
{
    public Guid ReservationId { get; set; }
    public string Outcome { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public bool Replayed { get; set; }
}
