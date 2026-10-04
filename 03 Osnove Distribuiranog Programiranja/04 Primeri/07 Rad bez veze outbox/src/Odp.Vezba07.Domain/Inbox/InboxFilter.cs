namespace Odp.Vezba07.Domain.Inbox;

public sealed class InboxFilter
{
    /// <summary>Pouzdana isporuka znači "najmanje jednom"; primalac zato mora da prepozna ponavljanje.</summary>
    public string Decide(bool alreadyApplied) =>
        alreadyApplied ? InboxCodes.DuplicateIgnored : InboxCodes.Applied;
}
