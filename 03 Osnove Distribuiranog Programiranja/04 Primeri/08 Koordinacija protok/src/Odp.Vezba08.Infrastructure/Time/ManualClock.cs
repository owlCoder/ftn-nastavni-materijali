using Odp.Vezba08.Application.Ports;

namespace Odp.Vezba08.Infrastructure.Time;

/// <summary>Sat koji pomera demonstracija ili test; istek lease-a se zadaje, ne čeka.</summary>
public sealed class ManualClock(DateTimeOffset start) : IClock
{
    public DateTimeOffset UtcNow { get; private set; } = start;

    public void Advance(TimeSpan by) => UtcNow += by;
}
