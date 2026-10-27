using NUnit.Framework;
using Oib.Vezba02.Application.Resources.Read;
using Oib.Vezba02.Domain.Access;
using Oib.Vezba02.Domain.Resources;
using Oib.Vezba02.Infrastructure.Audit;
using Oib.Vezba02.Infrastructure.Resources;

namespace Oib.Vezba02.Tests.Application;

public sealed class ReadResourceHandlerTests
{
    private static readonly ProtectedResource Record =
        new("rec-42", "ana", DataClassification.Confidential);

    private InMemoryResourceAccessAuditLog _auditLog = null!;
    private ReadResourceHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _auditLog = new InMemoryResourceAccessAuditLog();
        _handler = new ReadResourceHandler(
            new InMemoryProtectedResourceRepository([Record]),
            new ResourceReadPolicy(),
            _auditLog);
    }

    [Test]
    public void Read_WhenOwnerRequestsResource_ReturnsResource()
    {
        var result = _handler.Read(new ReadResourceQuery(Requester("ana"), Record.Id));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(ReadResourceOutcome.Granted));
            Assert.That(result.Resource, Is.EqualTo(Record));
        }
    }

    [Test]
    public void Read_WhenAnotherUserGuessesIdentifier_DeniesWithoutReturningData()
    {
        var result = _handler.Read(new ReadResourceQuery(Requester("marko"), Record.Id));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(ReadResourceOutcome.Denied));
            Assert.That(result.Code, Is.EqualTo(ResourceAccessCodes.NotOwner));
            Assert.That(result.Resource, Is.Null);
        }
    }

    [Test]
    public void Read_WhenResourceDoesNotExist_ReturnsNotFound()
    {
        var result = _handler.Read(new ReadResourceQuery(Requester("ana"), "rec-404"));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Outcome, Is.EqualTo(ReadResourceOutcome.NotFound));
            Assert.That(result.Resource, Is.Null);
        }
    }

    [Test]
    public void Read_RecordsEveryDecisionInAuditTrail()
    {
        _handler.Read(new ReadResourceQuery(Requester("ana"), Record.Id));
        _handler.Read(new ReadResourceQuery(Requester("marko"), Record.Id));

        Assert.That(
            _auditLog.Entries,
            Is.EqualTo(new[]
            {
                new ResourceAccessAuditEntry(
                    "ana", Record.Id, ReadResourceOutcome.Granted, ResourceAccessCodes.OwnerAccess),
                new ResourceAccessAuditEntry(
                    "marko", Record.Id, ReadResourceOutcome.Denied, ResourceAccessCodes.NotOwner)
            }));
    }

    private static Requester Requester(string actorId) =>
        new(actorId, new HashSet<string> { Permissions.ReadRecords });
}
