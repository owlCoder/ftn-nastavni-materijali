using Odp.Vezba01.Application.Ports;

namespace Odp.Vezba01.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
