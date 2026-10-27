namespace Odp.Vezba05.Domain.ReadModels;

/// <summary>Prikaz izveden iz zapisa; nije izvor istine i može kasniti za njim.</summary>
public sealed record StationView(
    string StationId,
    long LastSequence,
    DateTimeOffset LastMeasuredAt,
    double LastSignalStrengthDbm);
