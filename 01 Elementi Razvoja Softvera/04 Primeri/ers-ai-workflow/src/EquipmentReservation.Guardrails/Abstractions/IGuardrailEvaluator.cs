using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Abstractions;

public interface IGuardrailEvaluator
{
    GuardrailDecision Evaluate(ToolInvocation invocation);
}
