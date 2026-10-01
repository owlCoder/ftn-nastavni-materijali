using System.ComponentModel;
using EquipmentReservation.Mcp.Processes;
using EquipmentReservation.Mcp.Tools.Results;
using EquipmentReservation.Mcp.Workspace;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Tools;

[McpServerToolType]
public sealed class ProjectTools(
    IProjectStructureProvider structure,
    IProjectCommandRunner commands)
{
    [McpServerTool(
        Name = "get_project_structure",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns a safe, read-only list of project files.")]
    public string GetProjectStructure() => string.Join('\n', structure.ListFiles());

    [McpServerTool(
        Name = "get_git_diff",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns git diff for the current working tree. No files are modified.")]
    public async Task<string> GetGitDiff(CancellationToken cancellationToken)
    {
        var result = await commands.RunAsync(ProjectCommand.GitDiff, cancellationToken);
        return ToolResultJson.Serialize(GitDiffToolResult.From(result));
    }

    [McpServerTool(
        Name = "run_unit_tests",
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Runs the whole EquipmentReservation solution and returns a structured summary.")]
    public async Task<string> RunUnitTests(CancellationToken cancellationToken)
    {
        var result = await commands.RunAsync(ProjectCommand.RunUnitTests, cancellationToken);
        return ToolResultJson.Serialize(UnitTestToolResult.From(result));
    }
}
