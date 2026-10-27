using Oib.Vezba08.Domain.Events;
using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Domain.Correlation;
public interface IEventCorrelator
{
    Result<IReadOnlyList<CorrelationCase>> Correlate(IEnumerable<SecurityEvent>? events);
}
