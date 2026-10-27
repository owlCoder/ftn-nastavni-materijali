namespace Odp.Vezba08.Application.Ports;

/// <summary>Zaštićeni resurs: pamti najviši token koji je video.</summary>
public interface IScheduleStore
{
    long HighestToken(string resource);

    void Write(string resource, long token, string value);
}
