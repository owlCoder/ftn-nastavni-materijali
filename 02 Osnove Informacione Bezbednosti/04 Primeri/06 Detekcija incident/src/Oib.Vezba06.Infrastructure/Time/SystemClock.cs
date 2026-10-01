using Oib.Vezba06.Application.Ports;

namespace Oib.Vezba06.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
