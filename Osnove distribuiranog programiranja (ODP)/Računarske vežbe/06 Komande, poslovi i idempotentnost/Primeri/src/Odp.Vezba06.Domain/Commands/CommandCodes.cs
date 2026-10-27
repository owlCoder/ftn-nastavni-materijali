namespace Odp.Vezba06.Domain.Commands;

public static class CommandCodes
{
    public const string Sent = "command_sent";
    public const string AlreadyDispatched = "command_already_dispatched";
    public const string Acknowledged = "command_acknowledged";
    public const string LateAckReconciled = "late_ack_reconciled";
    public const string AckDuplicate = "ack_duplicate";
    public const string CommandUnknown = "command_unknown";
    public const string Retried = "command_retried";
    public const string AttemptsExhausted = "attempts_exhausted";
}
