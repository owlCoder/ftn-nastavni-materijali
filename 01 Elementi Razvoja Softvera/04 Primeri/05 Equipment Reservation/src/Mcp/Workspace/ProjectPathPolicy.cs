namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectPathPolicy
{
    private readonly ProjectRoot _root;

    public ProjectPathPolicy(ProjectRoot root)
    {
        _root = root;
    }

    private static readonly string[] HiddenDirectories = [".git", ".vs", "bin", "obj"];
    private static readonly string[] HiddenFiles = [".env", "secrets.json", "appsettings.secrets.json"];
    private static readonly StringComparison PathComparison = OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    public bool IsExposed(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return false;

        var segments = relativePath.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        var fileName = segments.Length > 0 ? segments[^1] : string.Empty;
        return segments.Length > 0 && !segments.Any(segment => segment is "." or "..") &&
            !segments.Any(segment => HiddenDirectories.Contains(segment, StringComparer.OrdinalIgnoreCase)) &&
            !HiddenFiles.Contains(fileName, StringComparer.OrdinalIgnoreCase) &&
            !fileName.StartsWith(".env.", StringComparison.OrdinalIgnoreCase);
    }

    public string ResolveExposedPath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(_root.FullPath) || !IsExposed(relativePath) || Path.IsPathRooted(relativePath))
            return string.Empty;

        try
        {
            var fullPath = Path.GetFullPath(Path.Combine(_root.FullPath, relativePath));
            var prefix = Path.TrimEndingDirectorySeparator(_root.FullPath) + Path.DirectorySeparatorChar;
            if (!fullPath.StartsWith(prefix, PathComparison))
                return string.Empty;

            // Refuse symbolic links and reparse points in every segment, not only at the final file.
            var current = _root.FullPath;
            foreach (var segment in Path.GetRelativePath(_root.FullPath, fullPath).Split(Path.DirectorySeparatorChar))
            {
                current = Path.Combine(current, segment);
                if (File.Exists(current) || Directory.Exists(current))
                {
                    if ((File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
                        return string.Empty;
                }
            }

            return fullPath;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            return string.Empty;
        }
    }
}
