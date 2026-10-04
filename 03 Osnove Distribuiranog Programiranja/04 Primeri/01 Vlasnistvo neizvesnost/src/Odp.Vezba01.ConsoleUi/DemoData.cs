using Odp.Vezba01.Domain.Liveness;
using Odp.Vezba01.Domain.Nodes;

namespace Odp.Vezba01.ConsoleUi;

public static class DemoData
{
    public static readonly DateTimeOffset Start = new(2027, 2, 1, 9, 0, 0, TimeSpan.Zero);

    public static readonly LivenessThresholds Thresholds =
        new(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90));

    public static readonly StationNode[] Nodes =
    [
        new("node-ns-1", "GS-NOVI-SAD", Start),
        new("node-bg-1", "GS-BEOGRAD", Start)
    ];
}
