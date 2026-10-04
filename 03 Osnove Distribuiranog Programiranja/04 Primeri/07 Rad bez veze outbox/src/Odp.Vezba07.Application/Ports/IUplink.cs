using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Application.Ports;

public interface IUplink
{
    /// <summary>Vraća true samo kada je stigla potvrda prijema; false ne znači da poruka nije stigla.</summary>
    bool TrySend(OutboxMessage message);
}
