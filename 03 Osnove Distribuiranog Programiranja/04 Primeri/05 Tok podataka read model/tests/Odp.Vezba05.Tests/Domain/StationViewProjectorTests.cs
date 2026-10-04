using NUnit.Framework;
using Odp.Vezba05.Domain.ReadModels;
using Odp.Vezba05.Domain.Telemetry;

namespace Odp.Vezba05.Tests.Domain;

public sealed class StationViewProjectorTests
{
    private static readonly DateTimeOffset Start = new(2027, 3, 1, 9, 0, 0, TimeSpan.Zero);

    private readonly StationViewProjector _projector = new();

    [Test]
    public void Apply_WhenThereIsNoViewYet_CreatesItFromTheReading()
    {
        var (view, changed) = _projector.Apply(null, Reading(1, -91));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(changed, Is.True);
            Assert.That(view, Is.EqualTo(new StationView("GS-NOVI-SAD", 1, Start.AddSeconds(5), -91)));
        }
    }

    [Test]
    public void Apply_WhenReadingIsNewer_ReplacesTheView()
    {
        var (current, _) = _projector.Apply(null, Reading(1, -91));

        var (view, changed) = _projector.Apply(current, Reading(2, -85));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(changed, Is.True);
            Assert.That(view.LastSequence, Is.EqualTo(2));
        }
    }

    [TestCase(3)]
    [TestCase(2)]
    public void Apply_WhenReadingIsNotNewer_KeepsTheView(long lateSequence)
    {
        var (current, _) = _projector.Apply(null, Reading(3, -88));

        var (view, changed) = _projector.Apply(current, Reading(lateSequence, -120));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(changed, Is.False);
            Assert.That(view, Is.EqualTo(current));
        }
    }

    private static TelemetryReading Reading(long sequence, double signal) =>
        new("GS-NOVI-SAD", sequence, Start.AddSeconds(5 * sequence), signal);
}
