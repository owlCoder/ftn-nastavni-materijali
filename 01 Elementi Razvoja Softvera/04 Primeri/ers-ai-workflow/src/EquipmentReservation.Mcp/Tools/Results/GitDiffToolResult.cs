using EquipmentReservation.Mcp.Processes;

namespace EquipmentReservation.Mcp.Tools.Results;

public sealed record GitDiffToolResult(
    bool Success,
    int ExitCode,
    string Diff,
    string Error)
{
    public static GitDiffToolResult From(ProcessResult result) =>
        new(result.Succeeded, result.ExitCode, result.StandardOutput, result.StandardError);
}
