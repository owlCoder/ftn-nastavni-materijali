using Oib.Vezba05.Domain.Operations;
using Oib.Vezba05.Domain.Sessions;

namespace Oib.Vezba05.Domain.StepUp;

public sealed class StepUpAuthenticationPolicy
{
    public StepUpDecision Evaluate(
        AuthSession session,
        RiskyOperation operation,
        DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(operation);

        if (session.Revoked)
            return new(false, false, StepUpCodes.SessionRevoked, "Sesija je opozvana.");
        if (session.MfaVerifiedAt is not { } verifiedAt)
            return new(false, true, StepUpCodes.MfaRequired, "Potrebna je MFA potvrda.");
        if (!IsFresh(verifiedAt, operation.MaximumMfaAge, now))
            return new(false, true, StepUpCodes.MfaStale, "MFA potvrda više nije dovoljno sveža.");

        return new(true, false, StepUpCodes.StepUpSatisfied, "Step-up uslov je ispunjen.");
    }

    private static bool IsFresh(
        DateTimeOffset verifiedAt,
        TimeSpan maximumAge,
        DateTimeOffset now) =>
        verifiedAt <= now && now - verifiedAt <= maximumAge;
}
