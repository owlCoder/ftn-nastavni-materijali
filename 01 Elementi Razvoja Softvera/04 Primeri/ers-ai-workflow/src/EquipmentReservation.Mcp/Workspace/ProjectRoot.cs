namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectRoot
{
    public ProjectRoot(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        FullPath = Path.GetFullPath(path);
    }

    public string FullPath { get; }
}
