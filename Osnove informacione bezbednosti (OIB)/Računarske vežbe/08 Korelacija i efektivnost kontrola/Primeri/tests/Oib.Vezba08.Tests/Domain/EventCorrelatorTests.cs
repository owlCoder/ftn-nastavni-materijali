using NUnit.Framework;
using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Events;

namespace Oib.Vezba08.Tests.Domain;

public sealed class EventCorrelatorTests
{
    private readonly EventCorrelator _correlator = new();

    [Test]
    public void Correlate_GroupsEventsThatShareCorrelationIdIntoOneCase()
    {
        SecurityEvent[] events =
        [
            new("corr-42", "login-failed", "ana", false),
            new("corr-42", "step-up-required", "ana", true),
            new("corr-42", "export-denied", "ana", true)
        ];

        var correlationCase = _correlator.Correlate(events).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(correlationCase.CorrelationId, Is.EqualTo("corr-42"));
            Assert.That(correlationCase.EventCount, Is.EqualTo(3));
            Assert.That(
                correlationCase.EventTypes,
                Is.EquivalentTo(new[] { "login-failed", "step-up-required", "export-denied" }));
        }
    }

    [Test]
    public void Correlate_KeepsUnrelatedEventsInSeparateCases()
    {
        SecurityEvent[] events =
        [
            new("corr-42", "login-failed", "ana", false),
            new("corr-77", "login-failed", "marko", false)
        ];

        var cases = _correlator.Correlate(events);

        Assert.That(
            cases.Select(correlationCase => correlationCase.CorrelationId),
            Is.EquivalentTo(new[] { "corr-42", "corr-77" }));
    }

    [Test]
    public void Correlate_CountsRepeatedEventTypeOnceInTypesButEveryTimeInCount()
    {
        SecurityEvent[] events =
        [
            new("corr-42", "login-failed", "ana", false),
            new("corr-42", "LOGIN-FAILED", "ana", false)
        ];

        var correlationCase = _correlator.Correlate(events).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(correlationCase.EventCount, Is.EqualTo(2));
            Assert.That(correlationCase.EventTypes, Has.Count.EqualTo(1));
        }
    }
}
