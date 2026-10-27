namespace Odp.Vezba03.Domain.Contacts;

public sealed record ContactRequest(string MissionId, string StationId, TimeSpan Duration);
