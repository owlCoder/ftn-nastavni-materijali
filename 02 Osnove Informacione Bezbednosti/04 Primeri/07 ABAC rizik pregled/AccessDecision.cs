namespace Oib.Vezba07;

public sealed record AccessDecision(bool Allowed, string Reason, DateTimeOffset ReviewAfter);

