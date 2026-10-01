using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Infrastructure.Resources;

public sealed class InMemoryProtectedResourceRepository(
    IEnumerable<ProtectedResource> resources) : IProtectedResourceRepository
{
    private readonly IReadOnlyDictionary<string, ProtectedResource> _resources =
        resources.ToDictionary(resource => resource.Id, StringComparer.Ordinal);

    public ProtectedResource? FindById(string resourceId) =>
        _resources.GetValueOrDefault(resourceId);
}
