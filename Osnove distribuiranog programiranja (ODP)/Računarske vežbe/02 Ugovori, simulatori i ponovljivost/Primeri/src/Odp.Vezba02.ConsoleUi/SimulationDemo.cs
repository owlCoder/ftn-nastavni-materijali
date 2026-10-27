using Odp.Vezba02.Application.Ingestion;
using Odp.Vezba02.Application.Ports;

namespace Odp.Vezba02.ConsoleUi;

public sealed class SimulationDemo(
    IStationSimulator simulator,
    IIngestSimulatedTelemetryUseCase ingest,
    TextWriter output)
{
    private const int MessageCount = 8;

    public void Run()
    {
        output.WriteLine("Poruke simulatora:");
        foreach (var message in simulator.Emit(MessageCount))
            output.WriteLine(
                $"  #{message.Sequence} {message.MeasuredAt:HH:mm:ss} {message.SignalStrengthDbm,6:0.0} dBm");

        var first = ingest.Run(MessageCount);
        var second = ingest.Run(MessageCount);

        output.WriteLine();
        output.WriteLine($"Prihvaćeno: {first.Accepted}, odbijeno: {first.Rejected.Count}");
        foreach (var rejected in first.Rejected)
            output.WriteLine($"  #{rejected.Sequence} -> {rejected.Code}");

        output.WriteLine();
        output.WriteLine(
            $"Ponovljeno pokretanje daje isti ishod: {first.Accepted == second.Accepted && first.Rejected.SequenceEqual(second.Rejected)}");
    }
}
