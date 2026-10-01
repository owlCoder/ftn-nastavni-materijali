namespace Oib.Vezba06.Domain.Signals;

public sealed record LoginAttempt(
    string SubjectId,
    string SourceIp,
    bool Succeeded,
    DateTimeOffset Timestamp);
