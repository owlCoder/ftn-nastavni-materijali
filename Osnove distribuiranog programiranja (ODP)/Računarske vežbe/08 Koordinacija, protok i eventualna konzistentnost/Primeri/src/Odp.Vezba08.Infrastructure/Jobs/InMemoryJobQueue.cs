using Odp.Vezba08.Application.Ports;

namespace Odp.Vezba08.Infrastructure.Jobs;

public sealed class InMemoryJobQueue : IJobQueue
{
    private readonly Queue<string> _jobs = new();

    public int Count => _jobs.Count;

    public void Enqueue(string jobId) => _jobs.Enqueue(jobId);

    public string Dequeue() => _jobs.Dequeue();
}
