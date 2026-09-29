using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Policies;

public sealed class SensitiveFileGuardrail : IToolGuardrail
{
    private static readonly string[] ForbiddenNames =
        [".env", "secrets.json", "appsettings.secrets.json"];

    public GuardrailDecision Evaluate(ToolInvocation invocation)
    {
        if (string.IsNullOrWhiteSpace(invocation.FilePath))
            return GuardrailDecision.Allow();

        var normalized = invocation.FilePath.Replace('\\', '/');
        return ForbiddenNames.Any(name =>
                normalized.EndsWith(name, StringComparison.OrdinalIgnoreCase))
            ? GuardrailDecision.Block(
                $"Reading or writing '{invocation.FilePath}' is blocked by project policy.")
            : GuardrailDecision.Allow();
    }
}
