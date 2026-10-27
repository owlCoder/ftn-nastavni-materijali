using Odp.Vezba07.Domain.Inbox;
using Odp.Vezba07.Domain.Outbox;

namespace Odp.Vezba07.Infrastructure.Center;

/// <summary>Prijemna strana u centru: svaki MessageId primenjuje jednom.</summary>
public sealed class CenterInbox(InboxFilter filter)
{
    private readonly HashSet<string> _applied = new(StringComparer.Ordinal);
    private readonly List<string> _payloads = [];

    public IReadOnlyList<string> AppliedPayloads => _payloads;

    public int DuplicatesIgnored { get; private set; }

    public string Receive(OutboxMessage message)
    {
        var code = filter.Decide(_applied.Contains(message.MessageId));
        if (code == InboxCodes.DuplicateIgnored)
        {
            DuplicatesIgnored++;
            return code;
        }

        _applied.Add(message.MessageId);
        _payloads.Add(message.Payload);
        return code;
    }
}
