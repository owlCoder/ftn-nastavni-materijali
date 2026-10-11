using System.ComponentModel;
using EquipmentReservation.Mcp.Interfaces;
using ModelContextProtocol.Server;

namespace EquipmentReservation.Mcp.Resources;

[McpServerResourceType]
public sealed class ProjectResources
{
    private readonly IProjectFileReader _files;

    public ProjectResources(IProjectFileReader files)
    {
        _files = files;
    }

    [McpServerResource(UriTemplate = "project://instructions", Name = "project_instructions", MimeType = "text/markdown")]
    [Description("Stable project rules for AI-assisted development.")]
    public string Instructions() => _files.ReadText("AGENTS.md").Content;

    [McpServerResource(UriTemplate = "project://readme", Name = "project_readme", MimeType = "text/markdown")]
    [Description("Teaching example README.")]
    public string Readme() => _files.ReadText("README.md").Content;
}
