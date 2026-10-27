using NUnit.Framework;
using Odp.Vezba08.Domain.Intake;

namespace Odp.Vezba08.Tests.Domain;

public sealed class IntakePolicyTests
{
    private readonly IntakePolicy _policy = new(capacity: 2);

    [TestCase(0, IntakeCodes.Accepted)]
    [TestCase(1, IntakeCodes.Accepted)]
    [TestCase(2, IntakeCodes.Overloaded)]
    public void Decide_AcceptsOnlyWhileTheQueueHasRoom(int queuedJobs, string code)
    {
        Assert.That(_policy.Decide(queuedJobs), Is.EqualTo(code));
    }
}
