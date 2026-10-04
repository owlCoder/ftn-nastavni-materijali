namespace Odp.Vezba06.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
