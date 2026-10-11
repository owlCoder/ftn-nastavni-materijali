using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Interfaces;

public interface IGuardrailEvaluator
{
    GuardrailDecision Evaluate(ToolInvocation invocation);
}
