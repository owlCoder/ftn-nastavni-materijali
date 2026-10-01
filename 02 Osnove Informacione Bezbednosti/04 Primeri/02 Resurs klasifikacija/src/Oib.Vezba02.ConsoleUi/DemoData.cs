using Oib.Vezba02.Domain.Access;
using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.ConsoleUi;

public static class DemoData
{
    public const string ConfidentialRecordId = "rec-42";
    public const string RestrictedRecordId = "rec-77";

    public static IReadOnlyList<ProtectedResource> Resources { get; } =
    [
        new ProtectedResource(ConfidentialRecordId, "ana", DataClassification.Confidential),
        new ProtectedResource(RestrictedRecordId, "ana", DataClassification.Restricted)
    ];

    public static Requester Owner { get; } =
        new("ana", new HashSet<string> { Permissions.ReadRecords });

    public static Requester OtherUser { get; } =
        new("marko", new HashSet<string> { Permissions.ReadRecords });

    public static Requester Auditor { get; } =
        new("jelena", new HashSet<string> { Permissions.ReadRecords, Permissions.ReadAnyRecord });
}
