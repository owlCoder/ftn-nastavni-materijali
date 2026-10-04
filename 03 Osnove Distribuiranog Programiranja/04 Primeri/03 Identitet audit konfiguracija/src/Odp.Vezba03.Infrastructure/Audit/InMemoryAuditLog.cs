using Odp.Vezba03.Application.Audit;
using Odp.Vezba03.Application.Ports;

namespace Odp.Vezba03.Infrastructure.Audit;

public sealed class InMemoryAuditLog : IAuditLog, IAuditTrail
{
    private readonly List<AuditEntry> _entries = [];

    public IReadOnlyList<AuditEntry> Entries => _entries;

    public void Record(AuditEntry entry) => _entries.Add(entry);
}
