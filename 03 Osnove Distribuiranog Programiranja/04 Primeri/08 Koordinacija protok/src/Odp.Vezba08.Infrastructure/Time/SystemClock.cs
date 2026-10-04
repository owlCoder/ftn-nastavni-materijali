using Odp.Vezba08.Application.Ports;

namespace Odp.Vezba08.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
