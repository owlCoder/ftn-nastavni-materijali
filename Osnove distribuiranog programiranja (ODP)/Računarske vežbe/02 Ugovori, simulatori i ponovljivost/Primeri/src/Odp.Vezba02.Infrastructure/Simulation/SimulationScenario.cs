namespace Odp.Vezba02.Infrastructure.Simulation;

/// <summary>Sve što određuje tok simulacije; isti scenario uvek daje iste poruke.</summary>
public sealed record SimulationScenario(
    string StationId,
    int Seed,
    DateTimeOffset Start,
    TimeSpan Interval,
    int FaultEvery);
