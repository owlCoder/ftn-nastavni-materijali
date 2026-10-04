using Odp.Vezba06.Application.Acknowledgement;
using Odp.Vezba06.Application.Dispatch;
using Odp.Vezba06.Application.Retries;
using Odp.Vezba06.Infrastructure.Devices;
using Odp.Vezba06.Infrastructure.Time;

namespace Odp.Vezba06.ConsoleUi;

public sealed class CommandDemo(
    IDispatchCommandUseCase dispatch,
    IRetryTimedOutCommandsUseCase retryTimedOut,
    IAcknowledgeCommandUseCase acknowledge,
    SimulatedDevice device,
    ManualClock clock,
    TextWriter output)
{
    private const string CommandId = "cmd-7";

    public void Run()
    {
        Dispatch("operator šalje komandu");
        Dispatch("operator ponavlja isti zahtev");

        clock.Advance(TimeSpan.FromSeconds(12));
        Retry("potvrda nije stigla 12 s");

        clock.Advance(TimeSpan.FromSeconds(12));
        Retry("potvrda nije stigla još 12 s");

        output.WriteLine($"zakasnela potvrda -> {acknowledge.Acknowledge(CommandId)}");
        output.WriteLine($"ista potvrda ponovo -> {acknowledge.Acknowledge(CommandId)}");

        output.WriteLine();
        output.WriteLine(
            $"Uređaj je primio {device.Deliveries} isporuke, a izvršio {device.ExecutedCommandIds.Count} komandu.");
    }

    private void Dispatch(string label)
    {
        var result = dispatch.Dispatch(CommandId, "antena-1", "rotate:120");
        output.WriteLine($"{label} -> {result.Code} (pokušaj {result.Command.Attempts})");
    }

    private void Retry(string label)
    {
        foreach (var outcome in retryTimedOut.Run())
            output.WriteLine($"{label} -> {outcome.CommandId}: {outcome.Code}");
    }
}
