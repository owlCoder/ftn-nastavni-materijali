namespace Oib.Vezba01.Application.Access;

public sealed record AccessAuditEntry(
    DateTimeOffset OccurredAt,
    string ActorId,
    string Permission,
    bool Allowed,
    string DecisionCode);
