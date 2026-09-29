namespace Oib.Vezba05;

public sealed record AuthSession(string SubjectId, DateTimeOffset IssuedAt, DateTimeOffset? MfaVerifiedAt, bool Revoked);

