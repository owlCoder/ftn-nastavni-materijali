using Odp.Vezba08.Domain.Leases;

namespace Odp.Vezba08.Application.Leases;

public interface IAcquireLeaseUseCase
{
    LeaseDecision Acquire(string resource, string candidateId);
}
