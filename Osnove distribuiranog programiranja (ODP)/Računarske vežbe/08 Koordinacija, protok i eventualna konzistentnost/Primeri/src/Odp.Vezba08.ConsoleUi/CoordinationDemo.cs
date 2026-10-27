using Odp.Vezba08.Application.Jobs;
using Odp.Vezba08.Application.Leases;
using Odp.Vezba08.Application.Schedules;
using Odp.Vezba08.Infrastructure.Schedules;
using Odp.Vezba08.Infrastructure.Time;

namespace Odp.Vezba08.ConsoleUi;

public sealed class CoordinationDemo(
    IAcquireLeaseUseCase acquireLease,
    IWriteScheduleUseCase writeSchedule,
    ISubmitJobUseCase submitJob,
    InMemoryScheduleStore schedules,
    ManualClock clock,
    TextWriter output)
{
    public void Run()
    {
        output.WriteLine("Koordinacija:");
        var workerA = Acquire("worker-a");
        Acquire("worker-b");
        Write("worker-a", workerA, "plan A");

        clock.Advance(TimeSpan.FromSeconds(45));
        output.WriteLine("  worker-a je zastao 45 s; lease je istekao");
        var workerB = Acquire("worker-b");
        Write("worker-b", workerB, "plan B");
        Write("worker-a", workerA, "zakasneli plan A");
        output.WriteLine($"  važeći raspored: {schedules.ValueOf(DemoData.Resource)}");

        output.WriteLine();
        output.WriteLine($"Protok (kapacitet reda {DemoData.QueueCapacity}):");
        foreach (var jobId in new[] { "job-1", "job-2", "job-3" })
            output.WriteLine($"  {jobId} -> {submitJob.Submit(jobId)}");
    }

    private long Acquire(string workerId)
    {
        var decision = acquireLease.Acquire(DemoData.Resource, workerId);
        output.WriteLine(
            $"  {workerId} traži lease -> {decision.Code} (vlasnik {decision.Lease.OwnerId}, token {decision.Lease.FencingToken})");
        return decision.Lease.FencingToken;
    }

    private void Write(string workerId, long token, string value) =>
        output.WriteLine(
            $"  {workerId} upisuje \"{value}\" sa tokenom {token} -> {writeSchedule.Write(DemoData.Resource, token, value)}");
}
