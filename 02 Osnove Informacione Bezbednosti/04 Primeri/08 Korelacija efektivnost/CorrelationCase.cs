namespace Oib.Vezba08;

public sealed record CorrelationCase(string CorrelationId, int EventCount, IReadOnlySet<string> EventTypes);

