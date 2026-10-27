using Odp.Vezba08.Application.Ports;

namespace Odp.Vezba08.Infrastructure.Schedules;

public sealed class InMemoryScheduleStore : IScheduleStore
{
    private readonly Dictionary<string, (long Token, string Value)> _schedules = new(StringComparer.Ordinal);

    public long HighestToken(string resource) =>
        _schedules.TryGetValue(resource, out var schedule) ? schedule.Token : 0;

    public string? ValueOf(string resource) =>
        _schedules.TryGetValue(resource, out var schedule) ? schedule.Value : null;

    public void Write(string resource, long token, string value) => _schedules[resource] = (token, value);
}
