namespace Odp.Vezba06.Application.Retries;

public sealed record RetryOutcome(string CommandId, string Code);
