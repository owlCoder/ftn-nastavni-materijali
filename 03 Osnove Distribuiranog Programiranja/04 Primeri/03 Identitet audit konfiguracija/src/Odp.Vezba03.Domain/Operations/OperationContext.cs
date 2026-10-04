namespace Odp.Vezba03.Domain.Operations;

/// <summary>Identitet jedne operacije: putuje kroz sve komponente koje je obrađuju.</summary>
public sealed record OperationContext(string CorrelationId, string ActorId);
