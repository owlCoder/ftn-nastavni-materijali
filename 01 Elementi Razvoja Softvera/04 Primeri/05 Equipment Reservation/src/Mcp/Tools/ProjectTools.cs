using System.ComponentModel;
using EquipmentReservation.Mcp.Interfaces;
using EquipmentReservation.Mcp.Models;
using EquipmentReservation.Mcp.Tools.Results;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Tools;

[McpServerToolType]
public sealed class ProjectTools
{
    private readonly IProjectStructureProvider _structure;
    private readonly IProjectCommandRunner _commands;

    public ProjectTools(IProjectStructureProvider structure, IProjectCommandRunner commands)
    {
        _structure = structure;
        _commands = commands;
    }

    [McpServerTool(
        Name = "get_project_structure",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns a safe, read-only list of project files.")]
    public string GetProjectStructure() => string.Join('\n', _structure.ListFiles());

    [McpServerTool(
        Name = "get_git_diff",
        ReadOnly = true,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns git diff for the current working tree. No files are modified.")]
    public async Task<string> GetGitDiff(CancellationToken cancellationToken)
    {
        var result = await _commands.RunAsync(ProjectCommand.GitDiff, cancellationToken);
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
        var result = await _commands.RunAsync(ProjectCommand.RunUnitTests, cancellationToken);
        return ToolResultJson.Serialize(UnitTestToolResult.From(result));
    }
}
