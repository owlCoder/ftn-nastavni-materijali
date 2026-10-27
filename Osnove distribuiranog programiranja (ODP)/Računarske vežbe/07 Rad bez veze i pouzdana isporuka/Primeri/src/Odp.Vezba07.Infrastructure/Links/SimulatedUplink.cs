using Odp.Vezba07.Application.Ports;
using Odp.Vezba07.Domain.Outbox;
using Odp.Vezba07.Infrastructure.Center;

namespace Odp.Vezba07.Infrastructure.Links;

/// <summary>Veza sa dva kvara: potpun prekid i potvrda izgubljena posle isporuke.</summary>
public sealed class SimulatedUplink(CenterInbox center) : IUplink
{
    public bool IsUp { get; set; } = true;

    public bool LoseNextAck { get; set; }

    public bool TrySend(OutboxMessage message)
    {
        if (!IsUp)
            return false;

        center.Receive(message);

        if (LoseNextAck)
        {
            LoseNextAck = false;
            return false;
        }

        return true;
    }
}
