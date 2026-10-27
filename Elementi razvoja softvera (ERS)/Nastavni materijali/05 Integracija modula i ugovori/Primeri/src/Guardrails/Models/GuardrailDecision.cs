namespace EquipmentReservation.Guardrails.Models;

public sealed class GuardrailDecision
{
    public bool Allowed { get; private init; }
    public string Reason { get; private init; } = string.Empty;

    public static GuardrailDecision Allow() => new() { Allowed = true };
    public static GuardrailDecision Block(string reason) => new() { Reason = reason };
}
