using Oib.Vezba01.Application.Access;

namespace Oib.Vezba01.Application.Ports;

public interface IAccessAuditTrail
{
    IReadOnlyList<AccessAuditEntry> Entries { get; }
}
