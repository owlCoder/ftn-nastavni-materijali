using System.ComponentModel;
using EquipmentReservation.Mcp.Workspace;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Tools;

[McpServerToolType]
public sealed class ProjectTools(ProjectWorkspace workspace)
{
    [McpServerTool(
        Name = "get_project_structure",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns a safe, read-only list of project files.")]
    public string GetProjectStructure() => workspace.GetStructure();

    [McpServerTool(
        Name = "get_git_diff",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns git diff for the current working tree. No files are modified.")]
    public async Task<string> GetGitDiff(CancellationToken cancellationToken)
    {
        var result = await workspace.RunFixedCommandAsync(
            "git",
            ["diff", "--", "."],
            cancellationToken);

        return workspace.ToJson(new
        {
            success = result.ExitCode == 0,
            result.ExitCode,
            diff = result.StandardOutput,
            error = result.StandardError
        });
    }

    [McpServerTool(
        Name = "run_unit_tests",
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Runs the whole EquipmentReservation solution and returns a structured summary.")]
    public async Task<string> RunUnitTests(CancellationToken cancellationToken)
    {
        var result = await workspace.RunFixedCommandAsync(
            "dotnet",
            ["test", "EquipmentReservation.sln", "--nologo", "--verbosity", "minimal"],
            cancellationToken);

        return workspace.ToJson(new
        {
            success = result.ExitCode == 0,
            result.ExitCode,
            stdout = result.StandardOutput,
            stderr = result.StandardError
        });
    }
}
