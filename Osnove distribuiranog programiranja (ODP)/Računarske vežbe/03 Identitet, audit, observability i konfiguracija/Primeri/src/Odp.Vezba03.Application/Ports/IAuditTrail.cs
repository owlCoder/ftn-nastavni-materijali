using Odp.Vezba03.Application.Audit;

namespace Odp.Vezba03.Application.Ports;

public interface IAuditTrail
{
    IReadOnlyList<AuditEntry> Entries { get; }
}
