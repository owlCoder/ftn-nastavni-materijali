namespace Odp.Vezba04.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
