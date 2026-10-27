using NUnit.Framework;
using Oib.Vezba06.Domain.Detection;
using Oib.Vezba06.Domain.Signals;
using Oib.Vezba06.Tests.TestData;

namespace Oib.Vezba06.Tests.Domain;

public sealed class RepeatedFailedLoginRuleTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

    private readonly RepeatedFailedLoginRule _rule = new(threshold: 3, TimeSpan.FromMinutes(10));

    [Test]
    public void Detect_WhenFailuresReachThreshold_RaisesSignalWithContext()
    {
        var signals = _rule.Detect(Attempts.Failures("ana", "203.0.113.10", 3, Now), Now);

        Assert.That(
            signals,
            Is.EqualTo(new[]
            {
                new SecuritySignal(RepeatedFailedLoginRule.Name, "ana", "203.0.113.10", 3)
            }));
    }

    [Test]
    public void Detect_WhenFailuresAreBelowThreshold_RaisesNothing()
    {
        var signals = _rule.Detect(Attempts.Failures("ana", "203.0.113.10", 2, Now), Now);

        Assert.That(signals, Is.Empty);
    }

    [Test]
    public void Detect_IgnoresSuccessfulLogins()
    {
        LoginAttempt[] attempts =
        [
            .. Attempts.Failures("ana", "203.0.113.10", 2, Now),
            new LoginAttempt("ana", "203.0.113.10", Succeeded: true, Now)
        ];

        Assert.That(_rule.Detect(attempts, Now), Is.Empty);
    }

    [Test]
    public void Detect_IgnoresFailuresOutsideTheTimeWindow()
    {
        var signals = _rule.Detect(
            Attempts.Failures("ana", "203.0.113.10", 5, Now.AddMinutes(-11)),
            Now);

        Assert.That(signals, Is.Empty);
    }

    [Test]
    public void Detect_DoesNotMergeDifferentSourceAddresses()
    {
        LoginAttempt[] attempts =
        [
            .. Attempts.Failures("ana", "203.0.113.10", 2, Now),
            .. Attempts.Failures("ana", "198.51.100.7", 2, Now)
        ];

        Assert.That(_rule.Detect(attempts, Now), Is.Empty);
    }

    [Test]
    public void Detect_RaisesOneSignalPerSuspiciousSubject()
    {
        LoginAttempt[] attempts =
        [
            .. Attempts.Failures("ana", "203.0.113.10", 5, Now),
            .. Attempts.Failures("marko", "198.51.100.7", 3, Now)
        ];

        var signals = _rule.Detect(attempts, Now);

        Assert.That(
            signals.Select(signal => signal.SubjectId),
            Is.EquivalentTo(new[] { "ana", "marko" }));
    }
}
