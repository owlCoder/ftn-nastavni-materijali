using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.ConsoleUi;

public static class DemoData
{
    public static readonly DateTimeOffset Start = new(2027, 2, 22, 9, 0, 0, TimeSpan.Zero);

    public static readonly InboxRules Rules = new(SupportedMajor: 1, TimeSpan.FromMinutes(5));

    public static MessageEnvelope Message(string id, int major, int minor, DateTimeOffset sentAt) =>
        new(id, new ContractVersion(major, minor), sentAt, $"telemetrija {id}");
}
