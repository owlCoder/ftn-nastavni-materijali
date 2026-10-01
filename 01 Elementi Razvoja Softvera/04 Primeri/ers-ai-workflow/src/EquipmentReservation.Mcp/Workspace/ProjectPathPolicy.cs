namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectPathPolicy(ProjectRoot root)
{
    private static readonly string[] HiddenDirectories = [".git", ".vs", "bin", "obj"];
    private static readonly string[] HiddenFiles = [".env"];

    private static readonly StringComparison PathComparison = OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase
        : StringComparison.Ordinal;

    public bool IsExposed(string relativePath)
    {
        var segments = relativePath.Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            StringSplitOptions.RemoveEmptyEntries);

        return segments.Length > 0 &&
               !segments.Any(IsHiddenDirectory) &&
               !HiddenFiles.Contains(segments[^1], StringComparer.OrdinalIgnoreCase);
    }

    public string ResolveExposedPath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ProjectAccessDeniedException("Relative path is required.");
        if (Path.IsPathRooted(relativePath))
            throw new ProjectAccessDeniedException("Only project-relative paths are allowed.");

        var fullPath = Path.GetFullPath(Path.Combine(root.FullPath, relativePath));
        var rootWithSeparator =
            root.FullPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(rootWithSeparator, PathComparison))
            throw new ProjectAccessDeniedException(
                "Path traversal outside the project root is blocked.");
        if (!IsExposed(Path.GetRelativePath(root.FullPath, fullPath)))
            throw new ProjectAccessDeniedException(
                "The requested path is not exposed by the MCP server.");

        return fullPath;
    }

    private static bool IsHiddenDirectory(string segment) =>
        HiddenDirectories.Contains(segment, StringComparer.OrdinalIgnoreCase);
}
