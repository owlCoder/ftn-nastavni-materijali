using Oib.Vezba02.Domain.Shared;
using Oib.Vezba02.Application.Resources.Read;

namespace Oib.Vezba02.Application.Ports;

public interface IResourceAccessAuditLog
{
    Result<bool> Record(ResourceAccessAuditEntry? entry);
}
