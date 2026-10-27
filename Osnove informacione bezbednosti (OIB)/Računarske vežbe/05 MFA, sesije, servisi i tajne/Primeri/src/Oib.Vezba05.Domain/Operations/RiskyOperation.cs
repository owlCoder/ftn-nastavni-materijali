namespace Oib.Vezba05.Domain.Operations;

public sealed record RiskyOperation(
    string Name,
    TimeSpan MaximumMfaAge);
