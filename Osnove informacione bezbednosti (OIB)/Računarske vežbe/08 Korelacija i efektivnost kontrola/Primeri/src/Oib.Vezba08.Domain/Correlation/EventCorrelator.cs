using Oib.Vezba08.Domain.Events;
using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Domain.Correlation;
public sealed class EventCorrelator : IEventCorrelator
{
    public Result<IReadOnlyList<CorrelationCase>> Correlate(IEnumerable<SecurityEvent>? events)
    {
        if (events is null) return Result<IReadOnlyList<CorrelationCase>>.Fail("MissingEvents");
        var items = events.ToArray();
        if (items.Any(e => e is null || string.IsNullOrWhiteSpace(e.CorrelationId) || string.IsNullOrWhiteSpace(e.Type)))
            return Result<IReadOnlyList<CorrelationCase>>.Fail("InvalidEvent");
        IReadOnlyList<CorrelationCase> cases = items.GroupBy(e => e.CorrelationId)
            .Select(g => new CorrelationCase(g.Key, g.Count(),
                g.Select(e => e.Type).ToHashSet(StringComparer.OrdinalIgnoreCase))).ToArray();
        return Result<IReadOnlyList<CorrelationCase>>.Ok(cases);
    }
}
