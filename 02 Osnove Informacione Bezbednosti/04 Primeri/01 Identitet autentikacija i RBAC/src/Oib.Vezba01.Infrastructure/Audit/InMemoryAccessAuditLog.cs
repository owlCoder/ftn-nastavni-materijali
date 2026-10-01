using Oib.Vezba01.Application.Access;
using Oib.Vezba01.Application.Ports;

namespace Oib.Vezba01.Infrastructure.Audit;

public sealed class InMemoryAccessAuditLog : IAccessAuditLog, IAccessAuditTrail
{
    private readonly List<AccessAuditEntry> _entries = [];

    public IReadOnlyList<AccessAuditEntry> Entries => _entries;

    public void Record(AccessAuditEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        _entries.Add(entry);
    }
}
