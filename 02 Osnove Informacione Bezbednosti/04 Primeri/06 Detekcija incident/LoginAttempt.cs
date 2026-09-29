namespace Oib.Vezba06;

public sealed record LoginAttempt(string SubjectId, string SourceIp, bool Succeeded, DateTimeOffset Timestamp);

