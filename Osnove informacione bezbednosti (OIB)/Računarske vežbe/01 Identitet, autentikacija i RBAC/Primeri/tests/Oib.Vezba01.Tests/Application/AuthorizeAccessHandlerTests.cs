using NUnit.Framework;
using Oib.Vezba01.Application.Access;
using Oib.Vezba01.Domain.Authorization;
using Oib.Vezba01.Domain.Identity;
using Oib.Vezba01.Infrastructure.Audit;
using Oib.Vezba01.Infrastructure.Authorization;
using Oib.Vezba01.Tests.TestDoubles;

namespace Oib.Vezba01.Tests.Application;

public sealed class AuthorizeAccessHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private InMemoryAccessAuditLog _auditLog = null!;
    private AuthorizeAccessHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _auditLog = new InMemoryAccessAuditLog();
        _handler = new AuthorizeAccessHandler(
            new InMemoryRoleCatalog(
            [
                new Role("Operator", new HashSet<string> { "reports:view" }),
                new Role("SecurityAdmin", new HashSet<string> { "reports:view", "reports:export" })
            ]),
            new RbacPolicy(),
            _auditLog,
            new FixedClock(Now));
    }

    [Test]
    public void Authorize_WhenOperatorViewsReports_Allows()
    {
        var decision = _handler.Authorize(new AccessRequest(Actor("Operator"), "reports:view"));

        Assert.That(decision.Allowed, Is.True);
    }

    [Test]
    public void Authorize_WhenOperatorExportsReports_Denies()
    {
        var decision = _handler.Authorize(new AccessRequest(Actor("Operator"), "reports:export"));

        Assert.That(decision.Allowed, Is.False);
    }

    [Test]
    public void Authorize_WhenSecurityAdminExportsReports_Allows()
    {
        var decision = _handler.Authorize(
            new AccessRequest(Actor("SecurityAdmin"), "reports:export"));

        Assert.That(decision.Allowed, Is.True);
    }

    [Test]
    public void Authorize_WhenRoleIsNotInCatalog_DeniesByDefault()
    {
        var decision = _handler.Authorize(new AccessRequest(Actor("Ghost"), "reports:view"));

        Assert.That(decision.Code, Is.EqualTo(AccessDecisionCodes.PermissionNotGranted));
    }

    [Test]
    public void Authorize_RecordsAllowedAndDeniedDecisionsInAuditTrail()
    {
        _handler.Authorize(new AccessRequest(Actor("Operator"), "reports:view"));
        _handler.Authorize(new AccessRequest(Actor("Operator"), "reports:export"));

        Assert.That(
            _auditLog.Entries,
            Is.EqualTo(new[]
            {
                new AccessAuditEntry(
                    Now, "ana", "reports:view", true, AccessDecisionCodes.GrantedByRole),
                new AccessAuditEntry(
                    Now, "ana", "reports:export", false, AccessDecisionCodes.PermissionNotGranted)
            }));
    }

    private static Actor Actor(params string[] roles) =>
        new("ana", IsAuthenticated: true, roles.ToHashSet());
}
