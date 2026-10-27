namespace EquipmentReservation.Guardrails.Models;

public sealed class ToolInvocation
{
    public ToolInvocation() { }

    public ToolInvocation(string? toolName, string? command, string? filePath, bool valid = true)
    {
        ToolName = toolName ?? string.Empty;
        Command = command ?? string.Empty;
        FilePath = filePath ?? string.Empty;
        Valid = valid;
    }

    public string ToolName { get; } = string.Empty;
    public string Command { get; } = string.Empty;
    public string FilePath { get; } = string.Empty;
    public bool Valid { get; }
}
