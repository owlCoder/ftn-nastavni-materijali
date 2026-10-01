using Oib.Vezba03.Application.Ports;

namespace Oib.Vezba03.Infrastructure.Correlation;

public sealed class GuidCorrelationIdGenerator : ICorrelationIdGenerator
{
    public string NewCorrelationId() => Guid.NewGuid().ToString("N");
}
