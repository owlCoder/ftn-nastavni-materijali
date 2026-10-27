using NUnit.Framework;
using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Shared;
using Oib.Vezba07.Application.Access;
using Oib.Vezba07.Domain.Review;
using Oib.Vezba07.Domain.Access;
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
    public void Evaluate_UsesAnAlternativePolicyAndSchedulesItsReviewDate()
    {
        var handler = new EvaluateAccessHandler(
            new AlwaysAllowPolicy(), _reviewSchedule, new FixedClock(Now));

        var decision = handler.Evaluate(new EvaluateAccessRequest(Contexts.Safe, "vlasnik"));

        Assert.That(decision.Code, Is.EqualTo("ALTERNATIVE"));
        Assert.That(_reviewSchedule.Items.Single().ReviewDue, Is.EqualTo(Now.AddDays(7)));
    }

    private sealed class AlwaysAllowPolicy : IRiskBasedAccessPolicy
    {
        public AccessDecision Evaluate(AccessContext? context, DateTimeOffset now) =>
            new(true, "ALTERNATIVE", "Odobreno prema drugoj politici.", now.AddDays(7));
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
    [Test]
    public void Evaluate_WhenReviewCannotBeScheduled_DeniesAccess()
    {
        var handler = new EvaluateAccessHandler(Contexts.Policy(), new FailingSchedule(), new FixedClock(Now));
        var result = handler.Evaluate(new EvaluateAccessRequest(Contexts.Safe, "vlasnik"));
        Assert.That(result.Allowed, Is.False);
        Assert.That(result.Code, Is.EqualTo(AccessDecisionCodes.ReviewSchedulingFailed));
    }

    private sealed class FailingSchedule : IAccessReviewSchedule
    {
        public Result<bool> Schedule(AccessReviewItem? item) => Result<bool>.Fail("StorageUnavailable");
    }
}
