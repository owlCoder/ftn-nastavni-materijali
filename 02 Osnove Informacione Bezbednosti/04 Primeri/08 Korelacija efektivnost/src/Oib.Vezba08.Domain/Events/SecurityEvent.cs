namespace Oib.Vezba08.Domain.Events;

public sealed record SecurityEvent(
    string CorrelationId,
    string Type,
    string SubjectId,
    bool Blocked);
