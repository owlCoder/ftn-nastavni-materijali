namespace Oib.Vezba06.Domain.Incidents;

public sealed record Incident(
    string Id,
    IncidentSeverity Severity,
    string Summary,
    IncidentStatus Status,
    DateTimeOffset OpenedAt);
