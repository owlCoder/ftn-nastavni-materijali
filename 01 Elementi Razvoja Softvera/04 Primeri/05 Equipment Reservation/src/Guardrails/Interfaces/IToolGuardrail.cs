using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Interfaces;

public interface IToolGuardrail
{
    GuardrailDecision Evaluate(ToolInvocation invocation);
}
