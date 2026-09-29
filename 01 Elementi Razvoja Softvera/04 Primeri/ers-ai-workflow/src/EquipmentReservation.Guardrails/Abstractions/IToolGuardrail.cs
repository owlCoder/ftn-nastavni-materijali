using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Abstractions;

public interface IToolGuardrail
{
    GuardrailDecision Evaluate(ToolInvocation invocation);
}
