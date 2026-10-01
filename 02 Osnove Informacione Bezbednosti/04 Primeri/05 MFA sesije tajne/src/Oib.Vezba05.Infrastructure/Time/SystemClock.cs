using Oib.Vezba05.Application.Ports;

namespace Oib.Vezba05.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
