using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Services;

public sealed class GuardrailEvaluator(IEnumerable<IToolGuardrail> guardrails)
{
    private readonly IReadOnlyList<IToolGuardrail> _guardrails = guardrails.ToArray();

    public GuardrailDecision Evaluate(ToolInvocation invocation)
    {
        foreach (var guardrail in _guardrails)
        {
            var decision = guardrail.Evaluate(invocation);
            if (!decision.Allowed)
                return decision;
        }

        return GuardrailDecision.Allow();
    }
}
