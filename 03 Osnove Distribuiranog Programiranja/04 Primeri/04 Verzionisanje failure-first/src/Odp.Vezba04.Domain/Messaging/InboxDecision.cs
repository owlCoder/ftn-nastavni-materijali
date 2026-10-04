namespace Odp.Vezba04.Domain.Messaging;

public sealed record InboxDecision(bool Process, string Code);
