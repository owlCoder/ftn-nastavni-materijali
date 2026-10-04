namespace Odp.Vezba08.Application.Schedules;

public interface IWriteScheduleUseCase
{
    string Write(string resource, long fencingToken, string value);
}
