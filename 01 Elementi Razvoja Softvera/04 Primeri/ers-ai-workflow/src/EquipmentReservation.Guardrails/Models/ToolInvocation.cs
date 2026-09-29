namespace EquipmentReservation.Guardrails.Models;

public sealed record ToolInvocation(
    string? ToolName,
    string? Command,
    string? FilePath);
