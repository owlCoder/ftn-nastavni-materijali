using Oib.Vezba06.Application.Ports;

namespace Oib.Vezba06.Tests.TestDoubles;

internal sealed class FixedClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
