using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Policies;

public sealed class DangerousCommandGuardrail : IToolGuardrail
{
    private static readonly string[] ForbiddenFragments =
    [
        "git push --force",
        "git push -f",
        "rm -rf",
        "Remove-Item -Recurse -Force",
        "format c:"
    ];

    public GuardrailDecision Evaluate(ToolInvocation invocation)
    {
        if (string.IsNullOrWhiteSpace(invocation.Command))
            return GuardrailDecision.Allow();

        return ForbiddenFragments.Any(fragment =>
                invocation.Command.Contains(fragment, StringComparison.OrdinalIgnoreCase))
            ? GuardrailDecision.Block(
                "Destructive or forceful command blocked by project policy.")
            : GuardrailDecision.Allow();
    }
}
