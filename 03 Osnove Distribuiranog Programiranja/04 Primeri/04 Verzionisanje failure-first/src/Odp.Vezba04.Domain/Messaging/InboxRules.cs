namespace Odp.Vezba04.Domain.Messaging;

public sealed record InboxRules(int SupportedMajor, TimeSpan MaxAge);
