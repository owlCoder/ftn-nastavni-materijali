using NUnit.Framework;
using Odp.Vezba02.Infrastructure.Simulation;

namespace Odp.Vezba02.Tests.Infrastructure;

public sealed class ScriptedStationSimulatorTests
{
    private static readonly SimulationScenario Scenario = new(
        "GS-NOVI-SAD",
        Seed: 7,
        new DateTimeOffset(2027, 2, 8, 9, 0, 0, TimeSpan.Zero),
        TimeSpan.FromSeconds(5),
        FaultEvery: 0);

    [Test]
    public void Emit_WithTheSameScenario_ProducesTheSameMessages()
    {
        var first = new ScriptedStationSimulator(Scenario).Emit(20);
        var second = new ScriptedStationSimulator(Scenario).Emit(20);

        Assert.That(second, Is.EqualTo(first));
    }

    [Test]
    public void Emit_WithAnotherSeed_ProducesDifferentSignal()
    {
        var first = new ScriptedStationSimulator(Scenario).Emit(20);
        var second = new ScriptedStationSimulator(Scenario with { Seed = 8 }).Emit(20);

        Assert.That(second, Is.Not.EqualTo(first));
    }

    [Test]
    public void Emit_NumbersMessagesAndSpacesThemByTheInterval()
    {
        var messages = new ScriptedStationSimulator(Scenario).Emit(3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(messages.Select(message => message.Sequence), Is.EqualTo(new long[] { 1, 2, 3 }));
            Assert.That(messages[2].MeasuredAt, Is.EqualTo(Scenario.Start.AddSeconds(10)));
        }
    }
}
