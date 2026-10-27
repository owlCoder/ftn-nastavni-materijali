using Odp.Vezba01.Application.Ports;

namespace Odp.Vezba01.Infrastructure.Time;

/// <summary>Sat koji pomera demonstracija ili test; vreme je kontrolisan ulaz, ne okolina.</summary>
public sealed class ManualClock(DateTimeOffset start) : IClock
{
    public DateTimeOffset UtcNow { get; private set; } = start;

    public void Advance(TimeSpan by) => UtcNow += by;
}
