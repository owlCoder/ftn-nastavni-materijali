using Odp.Vezba02.Domain.Shared;

namespace Odp.Vezba02.Domain.Contracts;

public sealed class TelemetryContractValidator
{
    public Result Validate(TelemetryMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.ContractVersion != TelemetryContract.Version)
            return Result.Fail(ContractErrorCodes.UnsupportedVersion);
        if (string.IsNullOrWhiteSpace(message.StationId))
            return Result.Fail(ContractErrorCodes.StationIdRequired);
        if (message.Sequence <= 0)
            return Result.Fail(ContractErrorCodes.SequenceMustBePositive);
        if (message.SignalStrengthDbm is < TelemetryContract.MinSignalDbm or > TelemetryContract.MaxSignalDbm)
            return Result.Fail(ContractErrorCodes.SignalOutOfRange);

        return Result.Ok();
    }
}
