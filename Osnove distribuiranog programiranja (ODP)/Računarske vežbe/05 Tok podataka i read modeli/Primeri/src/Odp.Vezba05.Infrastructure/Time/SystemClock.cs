using Odp.Vezba05.Application.Ports;

namespace Odp.Vezba05.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
