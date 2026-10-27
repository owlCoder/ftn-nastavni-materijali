namespace Odp.Vezba03.Application.Audit;

public sealed record AuditEntry(
    DateTimeOffset OccurredAt,
    string CorrelationId,
    string ActorId,
    string Action,
    string Target,
    string Outcome);
