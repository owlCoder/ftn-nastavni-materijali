using Oib.Vezba05.Application.Ports;
using Oib.Vezba05.Domain.StepUp;

namespace Oib.Vezba05.Application.StepUp;

public sealed class AuthorizeRiskyOperationHandler(
    ISessionStore sessions,
    StepUpAuthenticationPolicy policy,
    IClock clock) : IAuthorizeRiskyOperationUseCase
{
    public StepUpDecision Authorize(AuthorizeOperationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = sessions.FindById(request.SessionId);

        return session is null
            ? new(false, false, StepUpCodes.SessionNotFound, "Sesija ne postoji.")
            : policy.Evaluate(session, request.Operation, clock.UtcNow);
    }
}
