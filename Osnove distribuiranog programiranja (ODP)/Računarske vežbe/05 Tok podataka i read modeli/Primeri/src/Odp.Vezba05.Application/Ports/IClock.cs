namespace Odp.Vezba05.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
