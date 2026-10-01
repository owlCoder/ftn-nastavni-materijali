namespace Oib.Vezba01.Domain.Authorization;

public sealed record AccessDecision(
    bool Allowed,
    string Code,
    string Reason);
