namespace Odp.Vezba03.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
