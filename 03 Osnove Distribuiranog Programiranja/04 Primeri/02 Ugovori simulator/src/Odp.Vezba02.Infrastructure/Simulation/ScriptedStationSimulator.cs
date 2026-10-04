using Odp.Vezba02.Application.Ports;
using Odp.Vezba02.Domain.Contracts;

namespace Odp.Vezba02.Infrastructure.Simulation;

public sealed class ScriptedStationSimulator(SimulationScenario scenario) : IStationSimulator
{
    private const double NominalSignalDbm = -90;
    private const double FaultySignalDbm = 5;

    public IReadOnlyList<TelemetryMessage> Emit(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var random = new Random(scenario.Seed);

        return Enumerable.Range(1, count)
            .Select(sequence => new TelemetryMessage(
                TelemetryContract.Version,
                scenario.StationId,
                sequence,
                scenario.Start + scenario.Interval * (sequence - 1),
                SignalFor(sequence, random)))
            .ToArray();
    }

    private double SignalFor(int sequence, Random random)
    {
        var noise = random.Next(-20, 21);
        return IsFaulty(sequence) ? FaultySignalDbm : NominalSignalDbm + noise;
    }

    private bool IsFaulty(int sequence) =>
        scenario.FaultEvery > 0 && sequence % scenario.FaultEvery == 0;
}
