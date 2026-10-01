using Oib.Vezba07.Domain.Access;

namespace Oib.Vezba07.ConsoleUi;

public static class DemoData
{
    public const string BusinessOwner = "vlasnik-izvestaja";
    public const int DeniedFromRiskScore = 70;

    public static IReadOnlyList<string> ResponsibleRoles { get; } = ["Operator"];

    public static IReadOnlyList<string> TrustedLocations { get; } = ["office", "vpn"];

    public static TimeSpan GrantReviewInterval { get; } = TimeSpan.FromDays(30);

    public static AccessContext Baseline { get; } = new(
        SubjectId: "ana",
        Role: "Operator",
        Permission: "reports:export",
        ManagedDevice: true,
        Location: "office",
        RestrictedResource: true,
        RiskScore: 20);
}
