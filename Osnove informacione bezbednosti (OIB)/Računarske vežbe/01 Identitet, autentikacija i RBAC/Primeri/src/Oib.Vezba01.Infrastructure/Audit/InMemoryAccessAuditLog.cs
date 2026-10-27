using Oib.Vezba01.Domain.Shared;
using Oib.Vezba01.Application.Access;
using Oib.Vezba01.Application.Ports;

namespace Oib.Vezba01.Infrastructure.Audit;

public sealed class InMemoryAccessAuditLog : IAccessAuditLog, IAccessAuditTrail
{
    private readonly List<AccessAuditEntry> _entries = [];

    public IReadOnlyList<AccessAuditEntry> Entries => _entries;

    public Result<bool> Record(AccessAuditEntry? entry)
    {
        if (entry is null) return Result<bool>.Fail("InvalidAccessAuditEntry");
        _entries.Add(entry);
        return Result<bool>.Ok(true);
    }
}
