using Oib.Vezba07.Application.Ports;

namespace Oib.Vezba07.Tests.TestDoubles;

internal sealed class FixedClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
