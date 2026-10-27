using NUnit.Framework;
using Odp.Vezba03.Application.Audit;
using Odp.Vezba03.Application.Contacts;
using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Operations;
using Odp.Vezba03.Infrastructure.Audit;
using Odp.Vezba03.Infrastructure.Stations;
using Odp.Vezba03.Tests.TestDoubles;

namespace Odp.Vezba03.Tests.Application;

public sealed class ScheduleContactHandlerTests
{
    private static readonly DateTimeOffset Now = new(2027, 2, 15, 9, 0, 0, TimeSpan.Zero);
    private static readonly OperationContext Context = new("op-42", "ana");
    private static readonly ContactRequest Request = new("M-ARGUS", "GS-NOVI-SAD", TimeSpan.FromMinutes(10));

    private InMemoryAuditLog _auditLog = null!;
    private InMemoryStationGateway _stationGateway = null!;
    private ScheduleContactHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _auditLog = new InMemoryAuditLog();
        _stationGateway = new InMemoryStationGateway(["GS-NOVI-SAD"]);
        _handler = new ScheduleContactHandler(
            new ContactRequestValidator(new ContactLimits(TimeSpan.FromMinutes(15))),
            _stationGateway,
            _auditLog,
            new FixedClock(Now));
    }

    [Test]
    public void Schedule_CarriesTheSameCorrelationIdToTheStationAndTheAuditTrail()
    {
        var result = _handler.Schedule(Request, Context);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.EqualTo(new ScheduleContactResult(true, ContactCodes.Scheduled, "op-42")));
            Assert.That(_stationGateway.Calls.Single().CorrelationId, Is.EqualTo("op-42"));
            Assert.That(
                _auditLog.Entries.Single(),
                Is.EqualTo(new AuditEntry(
                    Now,
                    "op-42",
                    "ana",
                    "contact.schedule",
                    "GS-NOVI-SAD",
                    ContactCodes.Scheduled)));
        }
    }

    [Test]
    public void Schedule_WhenRequestIsInvalid_AuditsTheRejectionWithoutCallingTheStation()
    {
        var result = _handler.Schedule(Request with { Duration = TimeSpan.FromMinutes(40) }, Context);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Scheduled, Is.False);
            Assert.That(_stationGateway.Calls, Is.Empty);
            Assert.That(_auditLog.Entries.Single().Outcome, Is.EqualTo(ContactCodes.DurationTooLong));
        }
    }

    [Test]
    public void Schedule_WhenStationRefuses_AuditsTheOutcomeUnderTheSameCorrelationId()
    {
        var result = _handler.Schedule(Request with { StationId = "GS-BEOGRAD" }, Context);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Code, Is.EqualTo(ContactCodes.StationUnavailable));
            Assert.That(_stationGateway.Calls.Single().CorrelationId, Is.EqualTo("op-42"));
            Assert.That(_auditLog.Entries.Single().CorrelationId, Is.EqualTo("op-42"));
        }
    }
}
