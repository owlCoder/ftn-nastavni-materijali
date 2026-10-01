using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Domain.Access.Rules;

namespace Oib.Vezba07.Tests.TestData;

internal static class Contexts
{
    public static AccessContext Safe { get; } = new(
        SubjectId: "ana",
        Role: "Operator",
        Permission: "reports:export",
        ManagedDevice: true,
        Location: "office",
        RestrictedResource: true,
        RiskScore: 20);

    public static RiskBasedAccessPolicy Policy() =>
        new(
            [
                new BusinessRoleRule(["Operator"]),
                new ManagedDeviceRule(),
                new TrustedLocationRule(["office", "vpn"]),
                new RiskThresholdRule(deniedFromRiskScore: 70)
            ],
            TimeSpan.FromDays(30));
}
