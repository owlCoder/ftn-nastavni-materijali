using Oib.Vezba01.Application.Ports;

namespace Oib.Vezba01.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
