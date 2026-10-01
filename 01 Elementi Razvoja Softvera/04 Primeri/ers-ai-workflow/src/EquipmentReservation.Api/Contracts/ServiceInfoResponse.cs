namespace EquipmentReservation.Api.Contracts;

public sealed record ServiceInfoResponse(
    string Service,
    IReadOnlyList<string> Endpoints,
    Guid DemoEquipmentId);
