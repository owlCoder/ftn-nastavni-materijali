using Odp.Vezba08.Domain.Leases;

namespace Odp.Vezba08.Application.Ports;

public interface ILeaseStore
{
    Lease? Find(string resource);

    void Save(Lease lease);
}
