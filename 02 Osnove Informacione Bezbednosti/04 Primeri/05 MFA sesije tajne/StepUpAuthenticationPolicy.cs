namespace Oib.Vezba05;

public sealed class StepUpAuthenticationPolicy
{
    public StepUpDecision Evaluate(AuthSession session, RiskyOperation operation, DateTimeOffset now)
    {
        if (session.Revoked) return new(false, false, "Sesija je opozvana.");
        if (session.MfaVerifiedAt is null) return new(false, true, "Potrebna je MFA potvrda.");
        if (now - session.MfaVerifiedAt > operation.MaximumMfaAge)
            return new(false, true, "MFA potvrda više nije dovoljno sveža.");
        return new(true, false, "Step-up uslov je ispunjen.");
    }
}

