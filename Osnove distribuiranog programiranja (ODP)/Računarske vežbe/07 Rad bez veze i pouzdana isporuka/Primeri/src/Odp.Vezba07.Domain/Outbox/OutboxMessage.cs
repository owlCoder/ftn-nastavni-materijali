namespace Odp.Vezba07.Domain.Outbox;

/// <summary>Namera sačuvana lokalno pre slanja; preživljava prekid veze.</summary>
public sealed record OutboxMessage(long Position, string MessageId, string Payload, bool Delivered);
