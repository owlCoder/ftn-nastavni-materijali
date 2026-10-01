using Oib.Vezba01.Application.Access;

namespace Oib.Vezba01.Application.Ports;

public interface IAccessAuditLog
{
    void Record(AccessAuditEntry entry);
}
