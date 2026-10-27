namespace Odp.Vezba08.Application.Ports;

public interface IJobQueue
{
    int Count { get; }

    void Enqueue(string jobId);
}
