namespace EquipmentReservation.Guardrails.Models;

public sealed record GuardrailDecision(bool Allowed, string? Reason)
{
    public static GuardrailDecision Allow() => new(true, null);
    public static GuardrailDecision Block(string reason) => new(false, reason);
}
