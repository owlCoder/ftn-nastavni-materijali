namespace EquipmentReservation.Mcp.Models;

public sealed class UnitTestToolResult
{
    public bool Success { get; init; }
    public int ExitCode { get; init; } = -1;
    public string Stdout { get; init; } = string.Empty;
    public string Stderr { get; init; } = string.Empty;

    public static UnitTestToolResult From(ProcessResult result) => new()
    {
        Success = result.Succeeded,
        ExitCode = result.ExitCode,
        Stdout = result.StandardOutput,
        Stderr = result.StandardError
    };
}
