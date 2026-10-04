using Odp.Vezba07.Application.Ports;
using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Application.Recording;

/// <summary>Upis uspeva i kada veze nema: stanica čuva nameru, a slanje je odvojen korak.</summary>
public sealed class RecordMeasurementHandler(IOutboxStore outbox) : IRecordMeasurementUseCase
{
    public string Record(string messageId, string payload)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(messageId);
        ArgumentNullException.ThrowIfNull(payload);

        if (outbox.Find(messageId) is not null)
            return OutboxCodes.AlreadyQueued;

        outbox.Append(messageId, payload);
        return OutboxCodes.Queued;
    }
}
