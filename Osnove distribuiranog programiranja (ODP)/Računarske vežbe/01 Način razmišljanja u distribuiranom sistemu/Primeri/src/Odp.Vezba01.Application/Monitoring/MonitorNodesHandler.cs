using Odp.Vezba01.Application.Ports;
using Odp.Vezba01.Domain.Liveness;

namespace Odp.Vezba01.Application.Monitoring;

public sealed class MonitorNodesHandler(INodeRegistry registry, NodeLivenessPolicy policy, IClock clock)
    : IMonitorNodesUseCase
{
    public IReadOnlyList<NodeStatusView> Assess()
    {
        var now = clock.UtcNow;

        return registry.All()
            .Select(node => new NodeStatusView(node.NodeId, node.StationId, policy.Evaluate(node, now)))
            .ToArray();
    }
}
