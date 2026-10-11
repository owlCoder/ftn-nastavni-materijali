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
    public string Instructions() => files.ReadText("AGENTS.md");

    [McpServerResource(
        UriTemplate = "project://solution",
        Name = "project_solution",
        MimeType = "text/plain")]
    [Description("Teaching solution and its project references.")]
    public string Solution() => files.ReadText("EquipmentReservation.sln");
}
