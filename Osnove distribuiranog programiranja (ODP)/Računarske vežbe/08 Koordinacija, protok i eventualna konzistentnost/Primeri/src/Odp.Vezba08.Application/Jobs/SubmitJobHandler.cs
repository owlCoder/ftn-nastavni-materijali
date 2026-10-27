using Odp.Vezba08.Application.Ports;
using Odp.Vezba08.Domain.Intake;

namespace Odp.Vezba08.Application.Jobs;

public sealed class SubmitJobHandler(IJobQueue queue, IntakePolicy policy) : ISubmitJobUseCase
{
    public string Submit(string jobId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jobId);

        var code = policy.Decide(queue.Count);
        if (code == IntakeCodes.Accepted)
            queue.Enqueue(jobId);

        return code;
    }
}
