using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Policies;

public sealed class SensitiveFileGuardrail : IToolGuardrail
{
    private const string EnvironmentFile = ".env";

    private static readonly string[] ForbiddenNames =
        [EnvironmentFile, "secrets.json", "appsettings.secrets.json"];

    public GuardrailDecision Evaluate(ToolInvocation invocation)
    {
        if (string.IsNullOrWhiteSpace(invocation.FilePath))
            return GuardrailDecision.Allow();

        return IsSensitive(FileNameOf(invocation.FilePath))
            ? GuardrailDecision.Block(
                $"Reading or writing '{invocation.FilePath}' is blocked by project policy.")
            : GuardrailDecision.Allow();
    }

    private static string FileNameOf(string path)
    {
        var normalized = path.Replace('\\', '/').TrimEnd('/');
        return normalized[(normalized.LastIndexOf('/') + 1)..];
    }

    private static bool IsSensitive(string fileName) =>
        ForbiddenNames.Contains(fileName, StringComparer.OrdinalIgnoreCase) ||
        fileName.StartsWith(EnvironmentFile + ".", StringComparison.OrdinalIgnoreCase);
}
