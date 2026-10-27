namespace Oib.Vezba02.Domain.Access;

public sealed record ResourceAccessDecision(
    bool Allowed,
    string Code);
