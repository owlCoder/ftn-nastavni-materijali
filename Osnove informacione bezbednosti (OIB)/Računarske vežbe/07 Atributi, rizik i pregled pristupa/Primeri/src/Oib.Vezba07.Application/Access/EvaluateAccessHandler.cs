using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Application.Access;

public sealed class EvaluateAccessHandler(
    RiskBasedAccessPolicy policy,
    IAccessReviewSchedule reviewSchedule,
    IClock clock) : IEvaluateAccessUseCase
{
    public AccessDecision Evaluate(EvaluateAccessRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var decision = policy.Evaluate(request.Context, clock.UtcNow);

        if (decision.Allowed)
            reviewSchedule.Schedule(new AccessReviewItem(
                request.Context.SubjectId,
                request.Context.Permission,
                request.BusinessOwner,
                decision.ReviewAfter));

        return decision;
    }
}
