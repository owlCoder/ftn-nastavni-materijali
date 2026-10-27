using NUnit.Framework;
using Oib.Vezba05.Application.StepUp;
using Oib.Vezba05.Domain.Operations;
using Oib.Vezba05.Domain.Sessions;
using Oib.Vezba05.Domain.StepUp;
using Oib.Vezba05.Infrastructure.Sessions;
using Oib.Vezba05.Tests.TestDoubles;

namespace Oib.Vezba05.Tests.Application;

public sealed class AuthorizeRiskyOperationHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);
    private static readonly RiskyOperation Export = new("Izvoz", TimeSpan.FromMinutes(10));

    private readonly AuthorizeRiskyOperationHandler _handler = new(
        new InMemorySessionStore(
        [
            new AuthSession("fresh", "ana", Now.AddHours(-2), Now.AddMinutes(-2), false),
            new AuthSession("stale", "ana", Now.AddHours(-2), Now.AddMinutes(-45), false)
        ]),
        new StepUpAuthenticationPolicy(),
        new FixedClock(Now));

    [Test]
    public void Authorize_WhenSessionHasFreshMfa_Allows()
    {
        var decision = _handler.Authorize(new AuthorizeOperationRequest("fresh", Export));

        Assert.That(decision.Allowed, Is.True);
    }

    [Test]
    public void Authorize_WhenSessionHasStaleMfa_AsksForStepUp()
    {
        var decision = _handler.Authorize(new AuthorizeOperationRequest("stale", Export));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.RequiresMfa, Is.True);
        }
    }

    [Test]
    public void Authorize_WhenSessionIsUnknown_Denies()
    {
        var decision = _handler.Authorize(new AuthorizeOperationRequest("missing", Export));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.Code, Is.EqualTo(StepUpCodes.SessionNotFound));
        }
    }
}
