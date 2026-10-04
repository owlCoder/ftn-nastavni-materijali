namespace Odp.Vezba06.Domain.Commands;

public sealed class CommandLifecycle
{
    private readonly RetryRules _rules;

    public CommandLifecycle(RetryRules rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(rules.AckTimeout, TimeSpan.Zero);
        ArgumentOutOfRangeException.ThrowIfLessThan(rules.MaxAttempts, 1);
        _rules = rules;
    }

    public DeviceCommand Create(string commandId, string deviceId, string action, DateTimeOffset now) =>
        new(commandId, deviceId, action, CommandState.Sent, Attempts: 1, now);

    /// <summary>Izostanak potvrde znači da ishod nije poznat, ne da komanda nije izvršena.</summary>
    public bool HasTimedOut(DeviceCommand command, DateTimeOffset now) =>
        command.State == CommandState.Sent && now - command.LastSentAt >= _rules.AckTimeout;

    public bool CanRetry(DeviceCommand command) => command.Attempts < _rules.MaxAttempts;

    public DeviceCommand Retry(DeviceCommand command, DateTimeOffset now) =>
        command with { State = CommandState.Sent, Attempts = command.Attempts + 1, LastSentAt = now };

    public DeviceCommand GiveUp(DeviceCommand command) => command with { State = CommandState.TimedOut };

    /// <summary>Potvrda koja stigne posle isteka i dalje govori istinu o uređaju.</summary>
    public (DeviceCommand Command, string Code) Acknowledge(DeviceCommand command) =>
        command.State switch
        {
            CommandState.Sent =>
                (command with { State = CommandState.Acknowledged }, CommandCodes.Acknowledged),
            CommandState.TimedOut =>
                (command with { State = CommandState.Acknowledged }, CommandCodes.LateAckReconciled),
            _ => (command, CommandCodes.AckDuplicate)
        };
}
