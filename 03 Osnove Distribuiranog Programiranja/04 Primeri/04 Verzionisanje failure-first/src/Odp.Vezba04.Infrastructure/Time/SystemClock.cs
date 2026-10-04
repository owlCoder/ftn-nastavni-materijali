using Odp.Vezba04.Application.Ports;

namespace Odp.Vezba04.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
