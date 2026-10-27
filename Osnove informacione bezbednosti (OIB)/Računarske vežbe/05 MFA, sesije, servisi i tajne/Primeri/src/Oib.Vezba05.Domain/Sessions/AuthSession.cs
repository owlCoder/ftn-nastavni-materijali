namespace Oib.Vezba05.Domain.Sessions;

public sealed record AuthSession(
    string Id,
    string SubjectId,
    DateTimeOffset IssuedAt,
    DateTimeOffset? MfaVerifiedAt,
    bool Revoked);
