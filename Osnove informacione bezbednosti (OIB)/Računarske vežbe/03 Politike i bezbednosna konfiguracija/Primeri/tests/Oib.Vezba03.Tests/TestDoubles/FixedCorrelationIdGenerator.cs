using Oib.Vezba03.Application.Ports;

namespace Oib.Vezba03.Tests.TestDoubles;

internal sealed class FixedCorrelationIdGenerator(string correlationId) : ICorrelationIdGenerator
{
    public string NewCorrelationId() => correlationId;
}
