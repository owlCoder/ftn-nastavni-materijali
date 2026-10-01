namespace Oib.Vezba07.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
