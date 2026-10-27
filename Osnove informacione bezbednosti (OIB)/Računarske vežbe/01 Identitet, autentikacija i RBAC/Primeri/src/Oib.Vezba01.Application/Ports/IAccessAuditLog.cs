using Oib.Vezba01.Domain.Shared;
using Oib.Vezba01.Application.Access;

namespace Oib.Vezba01.Application.Ports;

public interface IAccessAuditLog
{
    Result<bool> Record(AccessAuditEntry? entry);
}
