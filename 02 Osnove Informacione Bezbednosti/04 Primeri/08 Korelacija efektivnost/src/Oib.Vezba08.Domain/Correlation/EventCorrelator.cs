using Oib.Vezba08.Domain.Events;

namespace Oib.Vezba08.Domain.Correlation;

public sealed class EventCorrelator
{
    public IReadOnlyList<CorrelationCase> Correlate(IEnumerable<SecurityEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);

        return events
            .GroupBy(securityEvent => securityEvent.CorrelationId)
            .Select(group => new CorrelationCase(
                group.Key,
                group.Count(),
                group.Select(securityEvent => securityEvent.Type)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)))
            .ToArray();
    }
}
