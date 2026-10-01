namespace Oib.Vezba08.Domain.Correlation;

public sealed record CorrelationCase(
    string CorrelationId,
    int EventCount,
    IReadOnlySet<string> EventTypes);
