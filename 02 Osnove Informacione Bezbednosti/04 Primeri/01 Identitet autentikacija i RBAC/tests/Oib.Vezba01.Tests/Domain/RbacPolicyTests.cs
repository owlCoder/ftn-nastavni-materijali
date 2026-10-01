using NUnit.Framework;
using Oib.Vezba01.Domain.Authorization;
using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.Tests.Domain;

public sealed class RbacPolicyTests
{
    private static readonly Role Operator = new("Operator", new HashSet<string> { "reports:view" });

    private readonly RbacPolicy _policy = new();

    [Test]
    public void Decide_WhenRoleGrantsPermission_Allows()
    {
        var decision = _policy.Decide(Authenticated("Operator"), "reports:view", [Operator]);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.True);
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.GrantedByRole));
        }
    }

    [Test]
    public void Decide_WhenNoRoleGrantsPermission_Denies()
    {
        var decision = _policy.Decide(Authenticated("Operator"), "reports:export", [Operator]);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.PermissionNotGranted));
        }
    }

    [Test]
    public void Decide_WhenActorHasNoRoles_DeniesByDefault()
    {
        var decision = _policy.Decide(Authenticated(), "reports:view", []);

        Assert.That(decision.Allowed, Is.False);
    }

    [Test]
    public void Decide_WhenActorIsNotAuthenticated_DeniesEvenIfRoleWouldGrant()
    {
        var actor = new Actor("nepoznat", IsAuthenticated: false, new HashSet<string> { "Operator" });

        var decision = _policy.Decide(actor, "reports:view", [Operator]);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.NotAuthenticated));
        }
    }

    private static Actor Authenticated(params string[] roles) =>
        new("ana", IsAuthenticated: true, roles.ToHashSet());
}
