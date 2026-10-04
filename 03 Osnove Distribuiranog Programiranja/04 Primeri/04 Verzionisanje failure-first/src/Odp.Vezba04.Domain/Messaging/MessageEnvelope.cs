namespace Odp.Vezba04.Domain.Messaging;

public sealed record MessageEnvelope(
    string MessageId,
    ContractVersion Version,
    DateTimeOffset SentAt,
    string Payload);
