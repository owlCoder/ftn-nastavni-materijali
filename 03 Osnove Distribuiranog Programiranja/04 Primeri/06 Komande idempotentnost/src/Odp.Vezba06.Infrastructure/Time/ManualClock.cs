using Odp.Vezba06.Application.Ports;

namespace Odp.Vezba06.Infrastructure.Time;

/// <summary>Sat koji pomera demonstracija ili test; istek se zadaje, ne čeka.</summary>
public sealed class ManualClock(DateTimeOffset start) : IClock
{
    public DateTimeOffset UtcNow { get; private set; } = start;

    public void Advance(TimeSpan by) => UtcNow += by;
}
