using NUnit.Framework;
using Oib.Vezba05.Domain.Operations;
using Oib.Vezba05.Domain.Sessions;
using Oib.Vezba05.Domain.StepUp;

namespace Oib.Vezba05.Tests.Domain;

public sealed class StepUpAuthenticationPolicyTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);
    private static readonly RiskyOperation Export = new("Izvoz", TimeSpan.FromMinutes(10));

    private readonly StepUpAuthenticationPolicy _policy = new();

    [Test]
    public void Evaluate_WhenMfaIsFresh_Allows()
    {
        var decision = _policy.Evaluate(Session(Now.AddMinutes(-2)), Export, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.True);
            Assert.That(decision.RequiresMfa, Is.False);
            Assert.That(decision.Code, Is.EqualTo(StepUpCodes.StepUpSatisfied));
        }
    }

    [Test]
    public void Evaluate_WhenMfaIsExactlyAtMaximumAge_Allows()
    {
        var decision = _policy.Evaluate(Session(Now.AddMinutes(-10)), Export, Now);

        Assert.That(decision.Allowed, Is.True);
    }

    [Test]
    public void Evaluate_WhenMfaIsOlderThanOperationAllows_RequiresNewMfa()
    {
        var decision = _policy.Evaluate(Session(Now.AddMinutes(-45)), Export, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.RequiresMfa, Is.True);
            Assert.That(decision.Code, Is.EqualTo(StepUpCodes.MfaStale));
        }
    }

    [Test]
    public void Evaluate_WhenSessionHasNoMfa_RequiresMfa()
    {
        var decision = _policy.Evaluate(Session(mfaVerifiedAt: null), Export, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.RequiresMfa, Is.True);
            Assert.That(decision.Code, Is.EqualTo(StepUpCodes.MfaRequired));
        }
    }

    [Test]
    public void Evaluate_WhenMfaTimestampIsInTheFuture_DoesNotTrustIt()
    {
        var decision = _policy.Evaluate(Session(Now.AddMinutes(5)), Export, Now);

        Assert.That(decision.Code, Is.EqualTo(StepUpCodes.MfaStale));
    }

    [Test]
    public void Evaluate_WhenSessionIsRevoked_DeniesWithoutOfferingMfa()
    {
        var decision = _policy.Evaluate(Session(Now.AddMinutes(-1), revoked: true), Export, Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.RequiresMfa, Is.False);
            Assert.That(decision.Code, Is.EqualTo(StepUpCodes.SessionRevoked));
        }
    }

    private static AuthSession Session(DateTimeOffset? mfaVerifiedAt, bool revoked = false) =>
        new("s-1", "ana", Now.AddHours(-2), mfaVerifiedAt, revoked);
}
