using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Access;
using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Application.Access;

public sealed class EvaluateAccessHandler(
    IRiskBasedAccessPolicy policy,
    IAccessReviewSchedule reviewSchedule,
    IClock clock) : IEvaluateAccessUseCase
{
    public AccessDecision Evaluate(EvaluateAccessRequest? request)
    {
        if (request?.Context is null || string.IsNullOrWhiteSpace(request.BusinessOwner) ||
            string.IsNullOrWhiteSpace(request.Context.SubjectId) || string.IsNullOrWhiteSpace(request.Context.Permission))
            return new(false, AccessDecisionCodes.InvalidRequest, "Neispravan zahtev za pristup.", clock.UtcNow);

        var decision = policy.Evaluate(request.Context, clock.UtcNow);

        if (decision.Allowed)
        {
            var scheduled = reviewSchedule.Schedule(new AccessReviewItem(
                request.Context.SubjectId, request.Context.Permission,
                request.BusinessOwner, decision.ReviewAfter));
            if (!scheduled.Success)
                return new(false, AccessDecisionCodes.ReviewSchedulingFailed,
                    "Nije moguće zakazati pregled pristupa.", clock.UtcNow);
        }

        return decision;
    }
}
