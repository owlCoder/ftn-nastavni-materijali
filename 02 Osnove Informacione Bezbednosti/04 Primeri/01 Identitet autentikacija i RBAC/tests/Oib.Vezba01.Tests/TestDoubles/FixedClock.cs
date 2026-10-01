using Oib.Vezba01.Application.Ports;

namespace Oib.Vezba01.Tests.TestDoubles;

internal sealed class FixedClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
