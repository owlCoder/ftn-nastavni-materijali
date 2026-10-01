using NUnit.Framework;
using Oib.Vezba06.Domain.Incidents;
using Oib.Vezba06.Domain.Signals;

namespace Oib.Vezba06.Tests.Domain;

public sealed class IncidentFactoryTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private readonly IncidentFactory _factory = new(new IncidentSeverityPolicy(highSeverityFrom: 5));

    [TestCase(3, IncidentSeverity.Medium)]
    [TestCase(4, IncidentSeverity.Medium)]
    [TestCase(5, IncidentSeverity.High)]
    public void Open_DerivesSeverityFromSignalStrength(int count, IncidentSeverity severity)
    {
        var incident = _factory.Open("INC-0001", Signal(count), Now);

        Assert.That(incident.Severity, Is.EqualTo(severity));
    }

    [Test]
    public void Open_CreatesOpenIncidentThatKeepsSignalContext()
    {
        var incident = _factory.Open("INC-0001", Signal(5), Now);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(incident.Id, Is.EqualTo("INC-0001"));
            Assert.That(incident.Status, Is.EqualTo(IncidentStatus.Open));
            Assert.That(incident.OpenedAt, Is.EqualTo(Now));
            Assert.That(incident.Summary, Does.Contain("ana").And.Contain("203.0.113.10"));
        }
    }

    private static SecuritySignal Signal(int count) =>
        new("repeated-failed-login", "ana", "203.0.113.10", count);
}
