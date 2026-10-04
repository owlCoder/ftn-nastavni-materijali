using Odp.Vezba03.Application.Audit;

namespace Odp.Vezba03.Application.Ports;

public interface IAuditLog
{
    void Record(AuditEntry entry);
}
