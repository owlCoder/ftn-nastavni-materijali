namespace EquipmentReservation.Mcp.Models;

public sealed class GitDiffToolResult
{
    public bool Success { get; init; }
    public int ExitCode { get; init; } = -1;
    public string Diff { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;

    public static GitDiffToolResult From(ProcessResult result) => new()
    {
        Success = result.Succeeded,
        ExitCode = result.ExitCode,
        Diff = result.StandardOutput,
        Error = result.StandardError
    };
}
