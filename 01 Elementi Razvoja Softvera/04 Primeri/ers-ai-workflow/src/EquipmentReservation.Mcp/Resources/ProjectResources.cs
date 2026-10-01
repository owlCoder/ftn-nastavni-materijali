using System.ComponentModel;
using EquipmentReservation.Mcp.Workspace;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Resources;

[McpServerResourceType]
public sealed class ProjectResources(IProjectFileReader files)
{
    [McpServerResource(
        UriTemplate = "project://instructions",
        Name = "project_instructions",
        MimeType = "text/markdown")]
    [Description("Stable project rules for AI-assisted development.")]
    public string Instructions() => files.ReadText(".ai/AI_INSTRUCTIONS.md");

    [McpServerResource(
        UriTemplate = "project://readme",
        Name = "project_readme",
        MimeType = "text/markdown")]
    [Description("Teaching example README.")]
    public string Readme() => files.ReadText("README.md");
}
