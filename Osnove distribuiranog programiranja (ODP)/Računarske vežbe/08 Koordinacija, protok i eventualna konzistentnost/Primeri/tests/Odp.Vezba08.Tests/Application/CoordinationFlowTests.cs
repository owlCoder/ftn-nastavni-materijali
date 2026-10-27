using NUnit.Framework;
using Odp.Vezba08.Application.Jobs;
using Odp.Vezba08.Application.Leases;
using Odp.Vezba08.Application.Schedules;
using Odp.Vezba08.Domain.Fencing;
using Odp.Vezba08.Domain.Intake;
using Odp.Vezba08.Domain.Leases;
using Odp.Vezba08.Infrastructure.Jobs;
using Odp.Vezba08.Infrastructure.Leases;
using Odp.Vezba08.Infrastructure.Schedules;
using Odp.Vezba08.Infrastructure.Time;

namespace Odp.Vezba08.Tests.Application;

public sealed class CoordinationFlowTests
{
    private const string Resource = "raspored:GS-NOVI-SAD";
    private static readonly DateTimeOffset Start = new(2027, 3, 22, 9, 0, 0, TimeSpan.Zero);

    private ManualClock _clock = null!;
    private InMemoryScheduleStore _schedules = null!;
    private AcquireLeaseHandler _acquire = null!;
    private WriteScheduleHandler _write = null!;

    [SetUp]
    public void SetUp()
    {
        _clock = new ManualClock(Start);
        _schedules = new InMemoryScheduleStore();
        _acquire = new AcquireLeaseHandler(new InMemoryLeaseStore(), new LeasePolicy(TimeSpan.FromSeconds(30)), _clock);
        _write = new WriteScheduleHandler(_schedules, new FencingPolicy());
    }

    [Test]
    public void Acquire_WhenTwoWorkersCompete_OnlyOneBecomesTheOwner()
    {
        var first = _acquire.Acquire(Resource, "worker-a");
        var second = _acquire.Acquire(Resource, "worker-b");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Granted, Is.True);
            Assert.That(second.Granted, Is.False);
            Assert.That(second.Lease.OwnerId, Is.EqualTo("worker-a"));
        }
    }

    [Test]
    public void Write_WhenFormerOwnerWakesUpAfterTakeover_IsRejectedByItsStaleToken()
    {
        var former = _acquire.Acquire(Resource, "worker-a").Lease;
        _clock.Advance(TimeSpan.FromSeconds(45));
        var current = _acquire.Acquire(Resource, "worker-b").Lease;
        _write.Write(Resource, current.FencingToken, "plan B");

        var code = _write.Write(Resource, former.FencingToken, "zakasneli plan A");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(FencingCodes.StaleToken));
            Assert.That(_schedules.ValueOf(Resource), Is.EqualTo("plan B"));
        }
    }

    [Test]
    public void Submit_WhenQueueIsFull_TellsTheSenderInsteadOfQueueingSilently()
    {
        var queue = new InMemoryJobQueue();
        var submit = new SubmitJobHandler(queue, new IntakePolicy(capacity: 2));

        string[] codes = [submit.Submit("job-1"), submit.Submit("job-2"), submit.Submit("job-3")];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                codes,
                Is.EqualTo(new[] { IntakeCodes.Accepted, IntakeCodes.Accepted, IntakeCodes.Overloaded }));
            Assert.That(queue.Count, Is.EqualTo(2));
        }
    }

    [Test]
    public void Submit_AfterAJobIsTakenFromTheQueue_AcceptsAgain()
    {
        var queue = new InMemoryJobQueue();
        var submit = new SubmitJobHandler(queue, new IntakePolicy(capacity: 1));
        submit.Submit("job-1");
        queue.Dequeue();

        Assert.That(submit.Submit("job-2"), Is.EqualTo(IntakeCodes.Accepted));
    }
}
