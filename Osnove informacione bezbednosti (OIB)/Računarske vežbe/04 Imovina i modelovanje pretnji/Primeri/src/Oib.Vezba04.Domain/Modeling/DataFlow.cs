namespace Oib.Vezba04.Domain.Modeling;

public sealed record DataFlow(
    string Name,
    Asset Asset,
    bool CrossesTrustBoundary);
