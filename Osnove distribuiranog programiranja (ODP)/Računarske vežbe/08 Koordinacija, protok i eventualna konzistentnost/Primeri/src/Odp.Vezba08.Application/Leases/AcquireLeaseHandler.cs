using Odp.Vezba08.Application.Ports;
using Odp.Vezba08.Domain.Leases;

namespace Odp.Vezba08.Application.Leases;

public sealed class AcquireLeaseHandler(ILeaseStore leases, LeasePolicy policy, IClock clock)
    : IAcquireLeaseUseCase
{
    public LeaseDecision Acquire(string resource, string candidateId)
    {
        var decision = policy.Acquire(leases.Find(resource), resource, candidateId, clock.UtcNow);
        if (decision.Granted)
            leases.Save(decision.Lease);

        return decision;
    }
}
