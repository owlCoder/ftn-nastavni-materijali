namespace Odp.Vezba06.Domain.Commands;

public sealed record RetryRules(TimeSpan AckTimeout, int MaxAttempts);
