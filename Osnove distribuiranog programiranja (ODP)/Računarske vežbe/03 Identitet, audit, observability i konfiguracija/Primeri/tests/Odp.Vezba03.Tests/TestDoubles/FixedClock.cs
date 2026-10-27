using Odp.Vezba03.Application.Ports;

namespace Odp.Vezba03.Tests.TestDoubles;

public sealed class FixedClock(DateTimeOffset now) : IClock
{
    public DateTimeOffset UtcNow { get; } = now;
}
