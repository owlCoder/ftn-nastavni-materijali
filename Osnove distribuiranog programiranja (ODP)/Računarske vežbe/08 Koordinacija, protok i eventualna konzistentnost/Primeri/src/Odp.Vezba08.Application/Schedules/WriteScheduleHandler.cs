using Odp.Vezba08.Application.Ports;
using Odp.Vezba08.Domain.Fencing;

namespace Odp.Vezba08.Application.Schedules;

public sealed class WriteScheduleHandler(IScheduleStore schedules, FencingPolicy policy)
    : IWriteScheduleUseCase
{
    public string Write(string resource, long fencingToken, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);
        ArgumentNullException.ThrowIfNull(value);

        var code = policy.Decide(fencingToken, schedules.HighestToken(resource));
        if (code == FencingCodes.WriteAccepted)
            schedules.Write(resource, fencingToken, value);

        return code;
    }
}
