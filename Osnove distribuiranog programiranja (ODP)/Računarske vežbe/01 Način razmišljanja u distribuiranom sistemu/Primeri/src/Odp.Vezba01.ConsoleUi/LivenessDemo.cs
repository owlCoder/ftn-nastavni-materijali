using Odp.Vezba01.Application.Heartbeats;
using Odp.Vezba01.Application.Monitoring;
using Odp.Vezba01.Infrastructure.Time;

namespace Odp.Vezba01.ConsoleUi;

public sealed class LivenessDemo(
    IReportHeartbeatUseCase reportHeartbeat,
    IMonitorNodesUseCase monitorNodes,
    ManualClock clock,
    TextWriter output)
{
    public void Run()
    {
        Show("POČETAK");

        clock.Advance(TimeSpan.FromSeconds(45));
        Report("node-ns-1", clock.UtcNow);
        Show("POSLE 45 s (javio se samo node-ns-1)");

        clock.Advance(TimeSpan.FromSeconds(60));
        Show("POSLE 105 s");

        Report("node-ns-1", DemoData.Start.AddSeconds(20));
        Show("STIGAO ZAKASNELI HEARTBEAT ZA node-ns-1");

        Report("node-bg-1", clock.UtcNow);
        Show("node-bg-1 SE PONOVO JAVIO");
    }

    private void Report(string nodeId, DateTimeOffset sentAt)
    {
        var outcome = reportHeartbeat.Report(nodeId, sentAt);
        output.WriteLine($"  heartbeat {nodeId} poslat u {sentAt:HH:mm:ss} -> {outcome.Code}");
    }

    private void Show(string label)
    {
        output.WriteLine($"{label} [{clock.UtcNow:HH:mm:ss}]");
        foreach (var view in monitorNodes.Assess())
            output.WriteLine(
                $"  {view.NodeId} ({view.StationId}): {view.Liveness.Status} | " +
                $"{view.Liveness.Code} | tišina {view.Liveness.Silence.TotalSeconds:0} s");
    }
}
