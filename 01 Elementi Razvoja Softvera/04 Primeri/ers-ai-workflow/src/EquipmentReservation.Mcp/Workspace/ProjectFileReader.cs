namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectFileReader(ProjectPathPolicy pathPolicy) : IProjectFileReader
{
    private static readonly HashSet<string> AllowedTextExtensions = new(
        StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".md", ".json", ".props", ".sln"
    };

    public string ReadText(string relativePath)
    {
        var fullPath = pathPolicy.ResolveExposedPath(relativePath);
        var extension = Path.GetExtension(fullPath);

        if (!AllowedTextExtensions.Contains(extension))
            throw new ProjectAccessDeniedException(
                $"File type '{extension}' is not exposed by the MCP server.");
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Project file was not found.", relativePath);

        return File.ReadAllText(fullPath);
    }
}
