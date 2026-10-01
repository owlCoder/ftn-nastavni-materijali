using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Application.Resources.Read;

namespace Oib.Vezba02.Infrastructure.Audit;

public sealed class InMemoryResourceAccessAuditLog : IResourceAccessAuditLog, IResourceAccessAuditTrail
{
    private readonly List<ResourceAccessAuditEntry> _entries = [];

    public IReadOnlyList<ResourceAccessAuditEntry> Entries => _entries;

    public void Record(ResourceAccessAuditEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        _entries.Add(entry);
    }
}
