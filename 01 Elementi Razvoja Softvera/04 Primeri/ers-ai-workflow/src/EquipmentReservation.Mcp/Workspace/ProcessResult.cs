namespace EquipmentReservation.Mcp.Workspace;

public sealed record ProcessResult(
    int ExitCode,
    string StandardOutput,
    string StandardError);
