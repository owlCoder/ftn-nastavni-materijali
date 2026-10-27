using Odp.Vezba08.Application.Ports;
using Odp.Vezba08.Domain.Leases;

namespace Odp.Vezba08.Infrastructure.Leases;

public sealed class InMemoryLeaseStore : ILeaseStore
{
    private readonly Dictionary<string, Lease> _leases = new(StringComparer.Ordinal);

    public Lease? Find(string resource) => _leases.GetValueOrDefault(resource);

    public void Save(Lease lease) => _leases[lease.Resource] = lease;
}
