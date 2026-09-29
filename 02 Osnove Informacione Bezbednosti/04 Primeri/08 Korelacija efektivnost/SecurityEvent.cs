namespace Oib.Vezba08;

public sealed record SecurityEvent(string CorrelationId, string Type, string SubjectId, bool Blocked);

