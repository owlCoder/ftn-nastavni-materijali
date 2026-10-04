using Odp.Vezba02.Infrastructure.Simulation;

namespace Odp.Vezba02.ConsoleUi;

public static class DemoData
{
    public static readonly SimulationScenario Scenario = new(
        "GS-NOVI-SAD",
        Seed: 42,
        new DateTimeOffset(2027, 2, 8, 9, 0, 0, TimeSpan.Zero),
        TimeSpan.FromSeconds(5),
        FaultEvery: 4);
}
