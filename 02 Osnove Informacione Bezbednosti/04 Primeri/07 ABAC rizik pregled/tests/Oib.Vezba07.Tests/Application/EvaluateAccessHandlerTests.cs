using NUnit.Framework;
using Oib.Vezba07.Application.Access;
using Oib.Vezba07.Domain.Review;
using Oib.Vezba07.Infrastructure.Review;
using Oib.Vezba07.Tests.TestData;
using Oib.Vezba07.Tests.TestDoubles;

namespace Oib.Vezba07.Tests.Application;

public sealed class EvaluateAccessHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private InMemoryAccessReviewSchedule _reviewSchedule = null!;
    private EvaluateAccessHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _reviewSchedule = new InMemoryAccessReviewSchedule();
        _handler = new EvaluateAccessHandler(
            Contexts.Policy(),
            _reviewSchedule,
            new FixedClock(Now));
    }

    [Test]
    public void Evaluate_WhenAccessIsGranted_SchedulesPeriodicReviewForBusinessOwner()
    {
        var decision = _handler.Evaluate(new EvaluateAccessRequest(Contexts.Safe, "vlasnik"));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.True);
            Assert.That(
                _reviewSchedule.Items,
                Is.EqualTo(new[]
                {
                    new AccessReviewItem("ana", "reports:export", "vlasnik", Now.AddDays(30))
                }));
        }
    }

    [Test]
    public void Evaluate_WhenAccessIsDenied_SchedulesNoReview()
    {
        var decision = _handler.Evaluate(
            new EvaluateAccessRequest(Contexts.Safe with { RiskScore = 85 }, "vlasnik"));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(_reviewSchedule.Items, Is.Empty);
        }
    }
}
