namespace Odp.Vezba08.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
