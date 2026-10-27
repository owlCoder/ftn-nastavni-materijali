using NUnit.Framework;
using Odp.Vezba06.Domain.Commands;

namespace Odp.Vezba06.Tests.Domain;

public sealed class CommandLifecycleTests
{
    private static readonly DateTimeOffset SentAt = new(2027, 3, 8, 9, 0, 0, TimeSpan.Zero);

    private readonly CommandLifecycle _lifecycle = new(new RetryRules(TimeSpan.FromSeconds(10), MaxAttempts: 2));

    [TestCase(9, false)]
    [TestCase(10, true)]
    public void HasTimedOut_ComparesWaitingTimeWithAckTimeout(int waitedSeconds, bool timedOut)
    {
        var command = _lifecycle.Create("cmd-1", "antena-1", "rotate:120", SentAt);

        Assert.That(_lifecycle.HasTimedOut(command, SentAt.AddSeconds(waitedSeconds)), Is.EqualTo(timedOut));
    }

    [Test]
    public void HasTimedOut_WhenCommandIsAlreadyAcknowledged_IsFalse()
    {
        var (acknowledged, _) = _lifecycle.Acknowledge(_lifecycle.Create("cmd-1", "antena-1", "rotate:120", SentAt));

        Assert.That(_lifecycle.HasTimedOut(acknowledged, SentAt.AddHours(1)), Is.False);
    }

    [Test]
    public void Retry_KeepsTheCommandIdAndCountsTheAttempt()
    {
        var command = _lifecycle.Create("cmd-1", "antena-1", "rotate:120", SentAt);

        var retried = _lifecycle.Retry(command, SentAt.AddSeconds(12));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(retried.CommandId, Is.EqualTo("cmd-1"));
            Assert.That(retried.Attempts, Is.EqualTo(2));
            Assert.That(_lifecycle.CanRetry(retried), Is.False);
        }
    }

    [Test]
    public void Acknowledge_WhenCommandWasGivenUp_ReconcilesItFromTheLateAck()
    {
        var givenUp = _lifecycle.GiveUp(_lifecycle.Create("cmd-1", "antena-1", "rotate:120", SentAt));

        var (command, code) = _lifecycle.Acknowledge(givenUp);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(CommandCodes.LateAckReconciled));
            Assert.That(command.State, Is.EqualTo(CommandState.Acknowledged));
        }
    }

    [Test]
    public void Acknowledge_WhenRepeated_ReportsDuplicateAndChangesNothing()
    {
        var (acknowledged, _) = _lifecycle.Acknowledge(_lifecycle.Create("cmd-1", "antena-1", "rotate:120", SentAt));

        var (command, code) = _lifecycle.Acknowledge(acknowledged);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(CommandCodes.AckDuplicate));
            Assert.That(command, Is.EqualTo(acknowledged));
        }
    }
}
