using EquipmentReservation.Mcp.Interfaces;
using EquipmentReservation.Mcp.Models;

namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectFileReader : IProjectFileReader
{
    private readonly ProjectPathPolicy _pathPolicy;

    public ProjectFileReader(ProjectPathPolicy pathPolicy)
    {
        _pathPolicy = pathPolicy;
    }

    private static readonly HashSet<string> AllowedTextExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".md", ".json", ".props", ".sln", ".editorconfig"
    };

    public ProjectFileReadResult ReadText(string relativePath)
    {
        var fullPath = _pathPolicy.ResolveExposedPath(relativePath);
        if (string.IsNullOrEmpty(fullPath) || !AllowedTextExtensions.Contains(Path.GetExtension(fullPath)))
            return ProjectFileReadResult.Fail("ProjectAccessDenied");
        if (!File.Exists(fullPath))
            return ProjectFileReadResult.Fail("ProjectFileNotFound");

        try
        {
            return ProjectFileReadResult.Ok(File.ReadAllText(fullPath));
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return ProjectFileReadResult.Fail("ProjectFileUnreadable");
        }
    }
}
