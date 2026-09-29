namespace EquipmentReservation.Mcp.Workspace;

internal static class ProjectRootLocator
{
    public static string Find(string startPath)
    {
        var current = new DirectoryInfo(Path.GetFullPath(startPath));
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Directory.Build.props")))
                return current.FullName;

            current = current.Parent;
        }

        throw new InvalidOperationException(
            "Project root containing Directory.Build.props was not found.");
    }
}
