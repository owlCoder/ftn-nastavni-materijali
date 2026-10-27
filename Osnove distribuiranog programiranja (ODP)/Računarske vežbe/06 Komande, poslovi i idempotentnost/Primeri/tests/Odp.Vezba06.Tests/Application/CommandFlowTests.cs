using NUnit.Framework;
using Odp.Vezba06.Application.Acknowledgement;
using Odp.Vezba06.Application.Dispatch;
using Odp.Vezba06.Application.Retries;
using Odp.Vezba06.Domain.Commands;
using Odp.Vezba06.Infrastructure.Commands;
using Odp.Vezba06.Infrastructure.Devices;
using Odp.Vezba06.Infrastructure.Time;

namespace Odp.Vezba06.Tests.Application;

public sealed class CommandFlowTests
{
    private static readonly DateTimeOffset Start = new(2027, 3, 8, 9, 0, 0, TimeSpan.Zero);

    private ManualClock _clock = null!;
    private InMemoryCommandStore _commands = null!;
    private SimulatedDevice _device = null!;
    private DispatchCommandHandler _dispatch = null!;
    private RetryTimedOutCommandsHandler _retry = null!;
    private AcknowledgeCommandHandler _acknowledge = null!;

    [SetUp]
    public void SetUp()
    {
        _clock = new ManualClock(Start);
        _commands = new InMemoryCommandStore();
        _device = new SimulatedDevice();
        var lifecycle = new CommandLifecycle(new RetryRules(TimeSpan.FromSeconds(10), MaxAttempts: 2));
        _dispatch = new DispatchCommandHandler(_commands, _device, lifecycle, _clock);
        _retry = new RetryTimedOutCommandsHandler(_commands, _device, lifecycle, _clock);
        _acknowledge = new AcknowledgeCommandHandler(_commands, lifecycle);
    }

    [Test]
    public void Dispatch_WhenTheSameCommandIdIsSubmittedTwice_SendsItOnce()
    {
        var first = _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
        var second = _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first.Code, Is.EqualTo(CommandCodes.Sent));
            Assert.That(second.Code, Is.EqualTo(CommandCodes.AlreadyDispatched));
            Assert.That(_device.Deliveries, Is.EqualTo(1));
        }
    }

    [Test]
    public void Run_WhenAckIsMissing_ResendsTheSameCommandAndTheDeviceExecutesItOnce()
    {
        _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
        _clock.Advance(TimeSpan.FromSeconds(12));

        var outcomes = _retry.Run();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcomes, Is.EqualTo(new[] { new RetryOutcome("cmd-1", CommandCodes.Retried) }));
            Assert.That(_device.Deliveries, Is.EqualTo(2));
            Assert.That(_device.ExecutedCommandIds, Is.EqualTo(new[] { "cmd-1" }));
        }
    }

    [Test]
    public void Run_WhenAttemptsAreExhausted_GivesUpWithoutSendingAgain()
    {
        _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
        _clock.Advance(TimeSpan.FromSeconds(12));
        _retry.Run();
        _clock.Advance(TimeSpan.FromSeconds(12));

        var outcomes = _retry.Run();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(outcomes.Single().Code, Is.EqualTo(CommandCodes.AttemptsExhausted));
            Assert.That(_device.Deliveries, Is.EqualTo(2));
            Assert.That(_commands.Find("cmd-1")!.State, Is.EqualTo(CommandState.TimedOut));
        }
    }

    [Test]
    public void Acknowledge_WhenAckArrivesAfterGivingUp_RecordsThatTheCommandWasExecuted()
    {
        _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
        _clock.Advance(TimeSpan.FromSeconds(12));
        _retry.Run();
        _clock.Advance(TimeSpan.FromSeconds(12));
        _retry.Run();

        var code = _acknowledge.Acknowledge("cmd-1");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(CommandCodes.LateAckReconciled));
            Assert.That(_commands.Find("cmd-1")!.State, Is.EqualTo(CommandState.Acknowledged));
        }
    }

    [Test]
    public void Acknowledge_WhenCommandIsUnknown_ReportsItWithoutCreatingAnything()
    {
        var code = _acknowledge.Acknowledge("cmd-x");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(code, Is.EqualTo(CommandCodes.CommandUnknown));
            Assert.That(_commands.All(), Is.Empty);
        }
    }
}
