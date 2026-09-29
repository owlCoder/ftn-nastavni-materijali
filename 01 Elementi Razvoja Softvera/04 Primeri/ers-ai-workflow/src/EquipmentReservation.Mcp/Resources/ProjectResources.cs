using System.ComponentModel;
using EquipmentReservation.Mcp.Workspace;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Resources;

[McpServerResourceType]
public sealed class ProjectResources(ProjectWorkspace workspace)
{
    [McpServerResource(
        UriTemplate = "project://instructions",
        Name = "project_instructions",
        MimeType = "text/markdown")]
    [Description("Stable project rules for AI-assisted development.")]
    public string Instructions() =>
        workspace.ReadProjectFile(".ai/AI_INSTRUCTIONS.md");

    [McpServerResource(
        UriTemplate = "project://readme",
        Name = "project_readme",
        MimeType = "text/markdown")]
    [Description("Teaching example README.")]
    public string Readme() =>
        workspace.ReadProjectFile("README.md");
}
