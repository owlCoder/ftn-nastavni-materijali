namespace Odp.Vezba02.Domain.Contracts;

/// <summary>Ugovor između stanice i centra: jedino što obe strane smeju da pretpostave.</summary>
public sealed record TelemetryMessage(
    string ContractVersion,
    string StationId,
    long Sequence,
    DateTimeOffset MeasuredAt,
    double SignalStrengthDbm);
