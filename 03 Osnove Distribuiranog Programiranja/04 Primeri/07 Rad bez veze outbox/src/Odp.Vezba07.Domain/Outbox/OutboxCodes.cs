namespace Odp.Vezba07.Domain.Outbox;

public static class OutboxCodes
{
    public const string Queued = "message_queued";
    public const string AlreadyQueued = "message_already_queued";
    public const string Flushed = "outbox_flushed";
    public const string DeliveryUnconfirmed = "delivery_unconfirmed";
    public const string NothingPending = "nothing_pending";
}
