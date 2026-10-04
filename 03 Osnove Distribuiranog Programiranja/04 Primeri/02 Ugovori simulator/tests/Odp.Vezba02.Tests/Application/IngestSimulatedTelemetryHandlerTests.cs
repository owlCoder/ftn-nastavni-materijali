using NUnit.Framework;
using Odp.Vezba02.Application.Ingestion;
using Odp.Vezba02.Domain.Contracts;
using Odp.Vezba02.Infrastructure.Simulation;

namespace Odp.Vezba02.Tests.Application;

public sealed class IngestSimulatedTelemetryHandlerTests
{
    private static readonly SimulationScenario Scenario = new(
        "GS-NOVI-SAD",
        Seed: 7,
        new DateTimeOffset(2027, 2, 8, 9, 0, 0, TimeSpan.Zero),
        TimeSpan.FromSeconds(5),
        FaultEvery: 3);

    [Test]
    public void Run_RejectsEveryFaultyMessageWithItsSequenceAndCode()
    {
        var report = HandlerFor(Scenario).Run(7);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.Accepted, Is.EqualTo(5));
            Assert.That(
                report.Rejected,
                Is.EqualTo(new[]
                {
                    new RejectedMessage(3, ContractErrorCodes.SignalOutOfRange),
                    new RejectedMessage(6, ContractErrorCodes.SignalOutOfRange)
                }));
        }
    }

    [Test]
    public void Run_WhenScenarioHasNoFaults_AcceptsEverything()
    {
        var report = HandlerFor(Scenario with { FaultEvery = 0 }).Run(7);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(report.Accepted, Is.EqualTo(7));
            Assert.That(report.Rejected, Is.Empty);
        }
    }

    private static IngestSimulatedTelemetryHandler HandlerFor(SimulationScenario scenario) =>
        new(new ScriptedStationSimulator(scenario), new TelemetryContractValidator());
}
