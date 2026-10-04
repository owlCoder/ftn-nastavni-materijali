using Odp.Vezba02.Application.Ingestion;
using Odp.Vezba02.Domain.Contracts;
using Odp.Vezba02.Infrastructure.Simulation;

namespace Odp.Vezba02.ConsoleUi;

public static class CompositionRoot
{
    public static SimulationDemo CreateDemo(TextWriter output)
    {
        var simulator = new ScriptedStationSimulator(DemoData.Scenario);

        return new SimulationDemo(
            simulator,
            new IngestSimulatedTelemetryHandler(simulator, new TelemetryContractValidator()),
            output);
    }
}
