using Odp.Vezba01.Application.Heartbeats;
using Odp.Vezba01.Application.Monitoring;
using Odp.Vezba01.Domain.Heartbeats;
using Odp.Vezba01.Domain.Liveness;
using Odp.Vezba01.Infrastructure.Nodes;
using Odp.Vezba01.Infrastructure.Time;

namespace Odp.Vezba01.ConsoleUi;

public static class CompositionRoot
{
    public static LivenessDemo CreateDemo(TextWriter output)
    {
        var clock = new ManualClock(DemoData.Start);
        var registry = new InMemoryNodeRegistry(DemoData.Nodes);

        return new LivenessDemo(
            new ReportHeartbeatHandler(registry, new HeartbeatPolicy()),
            new MonitorNodesHandler(registry, new NodeLivenessPolicy(DemoData.Thresholds), clock),
            clock,
            output);
    }
}
