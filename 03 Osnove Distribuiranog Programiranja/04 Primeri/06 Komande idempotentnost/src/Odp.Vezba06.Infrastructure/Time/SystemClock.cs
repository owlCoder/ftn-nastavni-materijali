using Odp.Vezba06.Application.Ports;

namespace Odp.Vezba06.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
