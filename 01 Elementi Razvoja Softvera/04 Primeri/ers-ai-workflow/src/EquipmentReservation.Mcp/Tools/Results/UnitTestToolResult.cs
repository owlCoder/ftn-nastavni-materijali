using EquipmentReservation.Mcp.Processes;

namespace EquipmentReservation.Mcp.Tools.Results;

public sealed record UnitTestToolResult(
    bool Success,
    int ExitCode,
    string Stdout,
    string Stderr)
{
    public static UnitTestToolResult From(ProcessResult result) =>
        new(result.Succeeded, result.ExitCode, result.StandardOutput, result.StandardError);
}
