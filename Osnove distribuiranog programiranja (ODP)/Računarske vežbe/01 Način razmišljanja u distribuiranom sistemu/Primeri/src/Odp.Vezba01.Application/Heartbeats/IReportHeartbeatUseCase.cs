using Odp.Vezba01.Domain.Heartbeats;

namespace Odp.Vezba01.Application.Heartbeats;

public interface IReportHeartbeatUseCase
{
    HeartbeatOutcome Report(string nodeId, DateTimeOffset sentAt);
}
