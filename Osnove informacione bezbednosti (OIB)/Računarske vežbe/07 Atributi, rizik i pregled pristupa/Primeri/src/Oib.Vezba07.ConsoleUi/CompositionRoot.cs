using Oib.Vezba07.Application.Access;
using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Domain.Access.Rules;
using Oib.Vezba07.Infrastructure.Review;
using Oib.Vezba07.Infrastructure.Time;

namespace Oib.Vezba07.ConsoleUi;

public static class CompositionRoot
{
    public static AccessDemo CreateDemo(TextWriter output)
    {
        var reviewSchedule = new InMemoryAccessReviewSchedule();
        var policy = new RiskBasedAccessPolicy(
            [
                new BusinessRoleRule(DemoData.ResponsibleRoles),
                new ManagedDeviceRule(),
                new TrustedLocationRule(DemoData.TrustedLocations),
                new RiskThresholdRule(DemoData.DeniedFromRiskScore)
            ],
            DemoData.GrantReviewInterval);

        return new AccessDemo(
            new EvaluateAccessHandler(policy, reviewSchedule, new SystemClock()),
            reviewSchedule,
            output);
    }
}
