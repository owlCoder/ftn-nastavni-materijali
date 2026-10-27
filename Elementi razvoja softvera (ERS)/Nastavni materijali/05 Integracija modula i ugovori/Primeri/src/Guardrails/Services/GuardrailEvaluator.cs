using EquipmentReservation.Guardrails.Interfaces;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Services;

public sealed class GuardrailEvaluator : IGuardrailEvaluator
{
    private readonly IReadOnlyList<IToolGuardrail> _guardrails;

    public GuardrailEvaluator(IEnumerable<IToolGuardrail> guardrails)
    {
        _guardrails = guardrails?.ToArray() ?? [];
    }

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
