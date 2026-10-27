using Odp.Vezba02.Application.Ports;
using Odp.Vezba02.Domain.Contracts;

namespace Odp.Vezba02.Application.Ingestion;

public sealed class IngestSimulatedTelemetryHandler(
    IStationSimulator simulator,
    TelemetryContractValidator validator) : IIngestSimulatedTelemetryUseCase
{
    public IngestionReport Run(int messageCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(messageCount);

        var rejected = new List<RejectedMessage>();
        var accepted = 0;

        foreach (var message in simulator.Emit(messageCount))
        {
            var validation = validator.Validate(message);
            if (validation.Success)
                accepted++;
            else
                rejected.Add(new RejectedMessage(message.Sequence, validation.Error!));
        }

        return new IngestionReport(accepted, rejected);
    }
}
