using EquipmentReservation.Mcp.Interfaces;

namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectStructureProvider : IProjectStructureProvider
{
    private readonly ProjectRoot _root;
    private readonly ProjectPathPolicy _pathPolicy;

    public ProjectStructureProvider(ProjectRoot root, ProjectPathPolicy pathPolicy)
    {
        _root = root;
        _pathPolicy = pathPolicy;
    }

    public IReadOnlyList<string> ListFiles()
    {
        if (_root.FullPath == string.Empty || !Directory.Exists(_root.FullPath))
            return Array.Empty<string>();

        var files = new List<string>();
        var directories = new Stack<string>();
        directories.Push(_root.FullPath);

        try
        {
            while (directories.TryPop(out var directory))
            {
                foreach (var subdirectory in Directory.EnumerateDirectories(directory))
                {
                    var relative = Path.GetRelativePath(_root.FullPath, subdirectory);
                    if (_pathPolicy.ResolveExposedPath(relative) != string.Empty)
                        directories.Push(subdirectory);
                }

                foreach (var file in Directory.EnumerateFiles(directory))
                {
                    var relative = Path.GetRelativePath(_root.FullPath, file);
                    if (_pathPolicy.ResolveExposedPath(relative) != string.Empty)
                        files.Add(relative);
                }
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return Array.Empty<string>();
        }

        files.Sort(StringComparer.OrdinalIgnoreCase);
        return files;
    }
}
