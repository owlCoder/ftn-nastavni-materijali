namespace EquipmentReservation.Api.Dtos;

public sealed class ServiceInfoDto
{
    public string Service { get; set; } = string.Empty;
    public IReadOnlyList<string> Endpoints { get; set; } = Array.Empty<string>();
    public Guid DemoEquipmentId { get; set; }
}
