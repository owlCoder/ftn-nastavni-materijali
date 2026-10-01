namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectStructureProvider(
    ProjectRoot root,
    ProjectPathPolicy pathPolicy) : IProjectStructureProvider
{
    public IReadOnlyList<string> ListFiles() =>
        Directory
            .EnumerateFiles(root.FullPath, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(root.FullPath, path))
            .Where(pathPolicy.IsExposed)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
}
