namespace Oib.Vezba07.Domain.Access;

public sealed record AccessDecision(
    bool Allowed,
    string Code,
    string Reason,
    DateTimeOffset ReviewAfter);
