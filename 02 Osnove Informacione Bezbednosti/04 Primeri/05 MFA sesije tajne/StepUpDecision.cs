namespace Oib.Vezba05;

public sealed record StepUpDecision(bool Allowed, bool RequiresMfa, string Reason);

