using NUnit.Framework;
using Oib.Vezba06.Application.Detection;
using Oib.Vezba06.Domain.Detection;
using Oib.Vezba06.Domain.Incidents;
using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Infrastructure.Incidents;
using Oib.Vezba06.Infrastructure.Signals;
using Oib.Vezba06.Tests.TestData;
using Oib.Vezba06.Tests.TestDoubles;

namespace Oib.Vezba06.Tests.Application;

public sealed class DetectIncidentsHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private InMemoryIncidentRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = new InMemoryIncidentRepository();

    [Test]
    public void Detect_WhenSubjectKeepsFailing_OpensAndStoresIncident()
    {
        var opened = Handler(Attempts.Failures("ana", "203.0.113.10", 5, Now)).Detect();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(opened, Has.Count.EqualTo(1));
            Assert.That(opened[0].Id, Is.EqualTo("INC-0001"));
            Assert.That(opened[0].Severity, Is.EqualTo(IncidentSeverity.High));
            Assert.That(opened[0].OpenedAt, Is.EqualTo(Now));
            Assert.That(_repository.Incidents, Is.EqualTo(opened));
        }
    }

    [Test]
    public void Detect_WhenNothingIsSuspicious_OpensNoIncident()
    {
        var opened = Handler(Attempts.Failures("ana", "203.0.113.10", 1, Now)).Detect();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(opened, Is.Empty);
            Assert.That(_repository.Incidents, Is.Empty);
        }
    }

    [Test]
    public void Detect_GivesEveryIncidentItsOwnIdentifier()
    {
        LoginAttempt[] attempts =
        [
            .. Attempts.Failures("ana", "203.0.113.10", 5, Now),
            .. Attempts.Failures("marko", "198.51.100.7", 3, Now)
        ];

        var opened = Handler(attempts).Detect();

        Assert.That(
            opened.Select(incident => incident.Id),
            Is.EqualTo(new[] { "INC-0001", "INC-0002" }));
    }

    private DetectIncidentsHandler Handler(IEnumerable<LoginAttempt> attempts) =>
        new(
            new InMemoryLoginAttemptSource(attempts),
            [new RepeatedFailedLoginRule(threshold: 3, TimeSpan.FromMinutes(10))],
            new IncidentFactory(new IncidentSeverityPolicy(highSeverityFrom: 5)),
            new SequentialIncidentIdGenerator(),
            _repository,
            new FixedClock(Now));
}
