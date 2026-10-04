namespace Odp.Vezba01.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
