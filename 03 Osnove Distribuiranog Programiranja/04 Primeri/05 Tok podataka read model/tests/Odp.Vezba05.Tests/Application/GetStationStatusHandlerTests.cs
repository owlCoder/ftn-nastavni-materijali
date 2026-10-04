using NUnit.Framework;
using Odp.Vezba05.Application.Queries;
using Odp.Vezba05.Domain.ReadModels;
using Odp.Vezba05.Infrastructure.ReadModels;
using Odp.Vezba05.Infrastructure.Time;

namespace Odp.Vezba05.Tests.Application;

public sealed class GetStationStatusHandlerTests
{
    private static readonly DateTimeOffset MeasuredAt = new(2027, 3, 1, 9, 0, 0, TimeSpan.Zero);

    private ManualClock _clock = null!;
    private InMemoryStationViewStore _views = null!;
    private GetStationStatusHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _clock = new ManualClock(MeasuredAt);
        _views = new InMemoryStationViewStore();
        _handler = new GetStationStatusHandler(_views, new FreshnessPolicy(TimeSpan.FromSeconds(30)), _clock);
    }

    [Test]
    public void Get_WhenNoNewReadingArrives_MarksTheViewAsStaleInsteadOfHidingIt()
    {
        _views.Save(new StationView("GS-NOVI-SAD", 5, MeasuredAt, -91));
        _clock.Advance(TimeSpan.FromMinutes(2));

        var status = _handler.Get("GS-NOVI-SAD");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(status!.IsStale, Is.True);
            Assert.That(status.Age, Is.EqualTo(TimeSpan.FromMinutes(2)));
            Assert.That(status.View.LastSequence, Is.EqualTo(5));
        }
    }

    [Test]
    public void Get_WhenStationHasNoViewYet_ReturnsNothing()
    {
        Assert.That(_handler.Get("GS-BEOGRAD"), Is.Null);
    }
}
