using Odp.Vezba03.Application.Ports;

namespace Odp.Vezba03.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
