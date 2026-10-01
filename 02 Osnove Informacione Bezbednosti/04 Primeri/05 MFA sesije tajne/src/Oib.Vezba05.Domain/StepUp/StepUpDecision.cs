namespace Oib.Vezba05.Domain.StepUp;

public sealed record StepUpDecision(
    bool Allowed,
    bool RequiresMfa,
    string Code,
    string Reason);
