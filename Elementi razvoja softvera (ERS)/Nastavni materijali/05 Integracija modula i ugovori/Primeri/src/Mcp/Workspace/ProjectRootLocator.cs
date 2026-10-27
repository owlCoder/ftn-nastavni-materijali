namespace EquipmentReservation.Mcp.Workspace;

public static class ProjectRootLocator
{
    private const string RootMarker = "Directory.Build.props";

    public static ProjectRoot Find(string startPath)
    {
        if (string.IsNullOrWhiteSpace(startPath))
            return new ProjectRoot();

        try
        {
            var current = new DirectoryInfo(Path.GetFullPath(startPath));
            while (current is not null)
            {
                if (File.Exists(Path.Combine(current.FullName, RootMarker)))
                    return new ProjectRoot(current.FullName);

                current = current.Parent;
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException)
        {
            return new ProjectRoot();
        }

        return new ProjectRoot();
    }
}
