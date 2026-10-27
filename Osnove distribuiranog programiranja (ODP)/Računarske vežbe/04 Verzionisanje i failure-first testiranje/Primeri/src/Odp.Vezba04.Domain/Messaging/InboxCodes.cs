namespace Odp.Vezba04.Domain.Messaging;

public static class InboxCodes
{
    public const string Accepted = "message_accepted";
    public const string Duplicate = "message_duplicate";
    public const string TooOld = "message_too_old";
    public const string UnsupportedMajorVersion = "message_major_version_unsupported";
}
