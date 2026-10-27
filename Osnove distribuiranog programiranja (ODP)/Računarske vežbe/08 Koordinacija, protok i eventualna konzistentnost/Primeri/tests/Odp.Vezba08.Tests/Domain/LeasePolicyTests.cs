using NUnit.Framework;
using Odp.Vezba08.Domain.Leases;

namespace Odp.Vezba08.Tests.Domain;

public sealed class LeasePolicyTests
{
    private const string Resource = "raspored:GS-NOVI-SAD";
    private static readonly DateTimeOffset Now = new(2027, 3, 22, 9, 0, 0, TimeSpan.Zero);

    private readonly LeasePolicy _policy = new(TimeSpan.FromSeconds(30));

    [Test]
    public void Acquire_WhenResourceIsFree_GrantsTheFirstToken()
    {
        var decision = _policy.Acquire(null, Resource, "worker-a", Now);

        Assert.That(decision, Is.EqualTo(new LeaseDecision(
            true,
            LeaseCodes.Granted,
            new Lease(Resource, "worker-a", 1, Now.AddSeconds(30)))));
    }

    [Test]
    public void Acquire_WhenAnotherOwnerHoldsAValidLease_Denies()
    {
        var held = new Lease(Resource, "worker-a", 1, Now.AddSeconds(30));

        var decision = _policy.Acquire(held, Resource, "worker-b", Now.AddSeconds(29));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Granted, Is.False);
            Assert.That(decision.Code, Is.EqualTo(LeaseCodes.HeldByAnother));
            Assert.That(decision.Lease, Is.EqualTo(held));
        }
    }

    [Test]
    public void Acquire_WhenLeaseHasExpired_HandsItOverWithAHigherToken()
    {
        var held = new Lease(Resource, "worker-a", 1, Now.AddSeconds(30));

        var decision = _policy.Acquire(held, Resource, "worker-b", Now.AddSeconds(30));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Code, Is.EqualTo(LeaseCodes.Granted));
            Assert.That(decision.Lease.OwnerId, Is.EqualTo("worker-b"));
            Assert.That(decision.Lease.FencingToken, Is.EqualTo(2));
        }
    }

    [Test]
    public void Acquire_WhenOwnerAsksAgain_RenewsWithoutChangingTheToken()
    {
        var held = new Lease(Resource, "worker-a", 1, Now.AddSeconds(30));

        var decision = _policy.Acquire(held, Resource, "worker-a", Now.AddSeconds(20));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Code, Is.EqualTo(LeaseCodes.Renewed));
            Assert.That(decision.Lease.FencingToken, Is.EqualTo(1));
            Assert.That(decision.Lease.ExpiresAt, Is.EqualTo(Now.AddSeconds(50)));
        }
    }
}
