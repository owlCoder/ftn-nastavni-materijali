using Oib.Vezba05.Application.Ports;

namespace Oib.Vezba05.Tests.TestDoubles;

internal sealed class FixedClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
