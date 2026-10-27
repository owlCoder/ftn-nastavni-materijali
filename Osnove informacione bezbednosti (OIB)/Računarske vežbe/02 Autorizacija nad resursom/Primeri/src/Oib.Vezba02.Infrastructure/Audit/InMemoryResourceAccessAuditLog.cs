using Oib.Vezba02.Domain.Shared;
using Oib.Vezba02.Application.Ports;
using Oib.Vezba02.Application.Resources.Read;

namespace Oib.Vezba02.Infrastructure.Audit;

public sealed class InMemoryResourceAccessAuditLog : IResourceAccessAuditLog, IResourceAccessAuditTrail
{
    private readonly List<ResourceAccessAuditEntry> _entries = [];

    public IReadOnlyList<ResourceAccessAuditEntry> Entries => _entries;

    public Result<bool> Record(ResourceAccessAuditEntry? entry)
    {
        if (entry is null) return Result<bool>.Fail("InvalidResourceAccessAuditEntry");
        _entries.Add(entry);
        return Result<bool>.Ok(true);
    }
}
