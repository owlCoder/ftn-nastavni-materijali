namespace EquipmentReservation.Mcp.Workspace;

public static class ProjectRootLocator
{
    private const string RootMarker = "Directory.Build.props";

    public static ProjectRoot Find(string startPath)
    {
        var current = new DirectoryInfo(Path.GetFullPath(startPath));
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, RootMarker)))
                return new ProjectRoot(current.FullName);

            current = current.Parent;
        }

        throw new InvalidOperationException(
            $"Project root containing {RootMarker} was not found.");
    }
}
