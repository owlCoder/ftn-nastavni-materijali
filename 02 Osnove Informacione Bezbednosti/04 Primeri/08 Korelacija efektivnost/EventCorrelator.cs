namespace Oib.Vezba08;

public sealed class EventCorrelator
{
    public IReadOnlyList<CorrelationCase> Correlate(IEnumerable<SecurityEvent> events) =>
        events.GroupBy(item => item.CorrelationId)
            .Select(group => new CorrelationCase(
                group.Key,
                group.Count(),
                group.Select(item => item.Type).ToHashSet(StringComparer.OrdinalIgnoreCase)))
            .ToArray();
}

