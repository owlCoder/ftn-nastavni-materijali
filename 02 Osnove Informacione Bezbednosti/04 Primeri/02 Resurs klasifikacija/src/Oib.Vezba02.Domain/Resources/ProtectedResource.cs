namespace Oib.Vezba02.Domain.Resources;

public sealed record ProtectedResource(
    string Id,
    string OwnerId,
    DataClassification Classification);
