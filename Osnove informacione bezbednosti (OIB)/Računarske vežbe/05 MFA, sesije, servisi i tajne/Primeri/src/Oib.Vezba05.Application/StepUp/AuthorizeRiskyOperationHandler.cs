using Oib.Vezba05.Application.Ports;
using Oib.Vezba05.Domain.StepUp;

namespace Oib.Vezba05.Application.StepUp;

public sealed class AuthorizeRiskyOperationHandler(
    ISessionStore sessions,
    IStepUpAuthenticationPolicy policy,
    IClock clock) : IAuthorizeRiskyOperationUseCase
{
    public StepUpDecision Authorize(AuthorizeOperationRequest? request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.SessionId) || request.Operation is null ||
            request.Operation.MaximumMfaAge <= TimeSpan.Zero)
            return new(false, false, StepUpCodes.InvalidRequest, "Neispravan zahtev za operaciju.");

        var session = sessions.FindById(request.SessionId);

        return session is null
            ? new(false, false, StepUpCodes.SessionNotFound, "Sesija ne postoji.")
            : policy.Evaluate(session, request.Operation, clock.UtcNow);
    }
}
