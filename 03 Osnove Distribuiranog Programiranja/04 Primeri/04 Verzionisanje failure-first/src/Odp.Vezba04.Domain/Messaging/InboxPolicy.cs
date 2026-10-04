namespace Odp.Vezba04.Domain.Messaging;

public sealed class InboxPolicy(InboxRules rules)
{
    /// <summary>Kašnjenje, duplikat i nova verzija su očekivani ulazi, ne izuzeci.</summary>
    public InboxDecision Decide(MessageEnvelope envelope, bool alreadyProcessed, DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (envelope.Version.Major != rules.SupportedMajor)
            return new(false, InboxCodes.UnsupportedMajorVersion);
        if (alreadyProcessed)
            return new(false, InboxCodes.Duplicate);
        if (now - envelope.SentAt > rules.MaxAge)
            return new(false, InboxCodes.TooOld);

        return new(true, InboxCodes.Accepted);
    }
}
