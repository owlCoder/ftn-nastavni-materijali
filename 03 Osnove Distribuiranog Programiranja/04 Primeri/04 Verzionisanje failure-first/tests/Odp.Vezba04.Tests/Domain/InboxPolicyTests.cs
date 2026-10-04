using NUnit.Framework;
using Odp.Vezba04.Domain.Messaging;

namespace Odp.Vezba04.Tests.Domain;

public sealed class InboxPolicyTests
{
    private static readonly DateTimeOffset Now = new(2027, 2, 22, 9, 0, 0, TimeSpan.Zero);

    private readonly InboxPolicy _policy = new(new InboxRules(SupportedMajor: 1, TimeSpan.FromMinutes(5)));

    [Test]
    public void Decide_WhenMessageIsNewAndFresh_Accepts()
    {
        var decision = _policy.Decide(Envelope(1, 0, Now), alreadyProcessed: false, Now);

        Assert.That(decision, Is.EqualTo(new InboxDecision(true, InboxCodes.Accepted)));
    }

    [Test]
    public void Decide_WhenMessageWasAlreadyProcessed_IgnoresTheDuplicate()
    {
        var decision = _policy.Decide(Envelope(1, 0, Now), alreadyProcessed: true, Now);

        Assert.That(decision, Is.EqualTo(new InboxDecision(false, InboxCodes.Duplicate)));
    }

    [Test]
    public void Decide_WhenMinorVersionIsNewer_StillAccepts()
    {
        var decision = _policy.Decide(Envelope(1, 7, Now), alreadyProcessed: false, Now);

        Assert.That(decision.Process, Is.True);
    }

    [TestCase(0)]
    [TestCase(2)]
    public void Decide_WhenMajorVersionDiffers_Rejects(int major)
    {
        var decision = _policy.Decide(Envelope(major, 0, Now), alreadyProcessed: false, Now);

        Assert.That(decision, Is.EqualTo(new InboxDecision(false, InboxCodes.UnsupportedMajorVersion)));
    }

    [Test]
    public void Decide_WhenMessageIsExactlyAtMaximumAge_Accepts()
    {
        var decision = _policy.Decide(Envelope(1, 0, Now.AddMinutes(-5)), alreadyProcessed: false, Now);

        Assert.That(decision.Process, Is.True);
    }

    [Test]
    public void Decide_WhenMessageIsOlderThanMaximumAge_Rejects()
    {
        var decision = _policy.Decide(Envelope(1, 0, Now.AddMinutes(-5).AddSeconds(-1)), alreadyProcessed: false, Now);

        Assert.That(decision, Is.EqualTo(new InboxDecision(false, InboxCodes.TooOld)));
    }

    [Test]
    public void Decide_WhenOldMessageIsAlsoADuplicate_ReportsTheDuplicate()
    {
        var decision = _policy.Decide(Envelope(1, 0, Now.AddHours(-1)), alreadyProcessed: true, Now);

        Assert.That(decision.Code, Is.EqualTo(InboxCodes.Duplicate));
    }

    private static MessageEnvelope Envelope(int major, int minor, DateTimeOffset sentAt) =>
        new("m-1", new ContractVersion(major, minor), sentAt, "payload");
}
