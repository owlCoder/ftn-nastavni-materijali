using Odp.Vezba01.Application.Ports;
using Odp.Vezba01.Domain.Heartbeats;

namespace Odp.Vezba01.Application.Heartbeats;

public sealed class ReportHeartbeatHandler(INodeRegistry registry, HeartbeatPolicy policy)
    : IReportHeartbeatUseCase
{
    public HeartbeatOutcome Report(string nodeId, DateTimeOffset sentAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nodeId);

        var node = registry.Find(nodeId);
        if (node is null)
            return new(false, HeartbeatCodes.NodeUnknown);

        var (updated, outcome) = policy.Apply(node, sentAt);
        if (outcome.Accepted)
            registry.Save(updated);

        return outcome;
    }
}
