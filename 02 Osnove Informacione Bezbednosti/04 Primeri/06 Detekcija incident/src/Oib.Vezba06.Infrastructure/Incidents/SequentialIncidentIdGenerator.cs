using Oib.Vezba06.Application.Ports;

namespace Oib.Vezba06.Infrastructure.Incidents;

public sealed class SequentialIncidentIdGenerator : IIncidentIdGenerator
{
    private int _lastNumber;

    public string NextId() => $"INC-{Interlocked.Increment(ref _lastNumber):D4}";
}
