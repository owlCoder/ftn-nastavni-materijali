using NUnit.Framework;
using Odp.Vezba05.Application.Ingestion;
using Odp.Vezba05.Domain.ReadModels;
using Odp.Vezba05.Domain.Telemetry;
using Odp.Vezba05.Infrastructure.ReadModels;
using Odp.Vezba05.Infrastructure.Telemetry;

namespace Odp.Vezba05.Tests.Application;

public sealed class IngestTelemetryHandlerTests
{
    private static readonly DateTimeOffset Start = new(2027, 3, 1, 9, 0, 0, TimeSpan.Zero);

    private InMemoryTelemetryLog _log = null!;
    private InMemoryStationViewStore _views = null!;
    private IngestTelemetryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _log = new InMemoryTelemetryLog();
        _views = new InMemoryStationViewStore();
        _handler = new IngestTelemetryHandler(
            new TelemetryReadingValidator(),
            _log,
            new StationViewProjector(),
            _views);
    }

    [Test]
    public void Ingest_WhenOlderReadingArrivesLate_KeepsItInTheLogButNotInTheView()
    {
        _handler.Ingest(Reading(3, -88));

        var outcome = _handler.Ingest(Reading(2, -97));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome, Is.EqualTo(new IngestOutcome(true, TelemetryCodes.Stored, ViewChanged: false)));
            Assert.That(_log.ForStation("GS-NOVI-SAD"), Has.Count.EqualTo(2));
            Assert.That(_views.Find("GS-NOVI-SAD")!.LastSequence, Is.EqualTo(3));
        }
    }

    [Test]
    public void Ingest_WhenReadingBreaksTheContract_StoresNothing()
    {
        var outcome = _handler.Ingest(Reading(0, -90));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcome, Is.EqualTo(
                new IngestOutcome(false, TelemetryCodes.SequenceMustBePositive, ViewChanged: false)));
            Assert.That(_log.ForStation("GS-NOVI-SAD"), Is.Empty);
            Assert.That(_views.Find("GS-NOVI-SAD"), Is.Null);
        }
    }

    private static TelemetryReading Reading(long sequence, double signal) =>
        new("GS-NOVI-SAD", sequence, Start.AddSeconds(5 * sequence), signal);
}
