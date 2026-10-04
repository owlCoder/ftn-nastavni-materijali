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

namespace Odp.Vezba08.ConsoleUi;

public static class CompositionRoot
{
    public static CoordinationDemo CreateDemo(TextWriter output)
    {
        var clock = new ManualClock(DemoData.Start);
        var schedules = new InMemoryScheduleStore();

        return new CoordinationDemo(
            new AcquireLeaseHandler(new InMemoryLeaseStore(), new LeasePolicy(DemoData.LeaseDuration), clock),
            new WriteScheduleHandler(schedules, new FencingPolicy()),
            new SubmitJobHandler(new InMemoryJobQueue(), new IntakePolicy(DemoData.QueueCapacity)),
            schedules,
            clock,
            output);
    }
}
