using Oib.Vezba07.Application.Ports;

namespace Oib.Vezba07.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
