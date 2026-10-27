namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectRoot
{
    public ProjectRoot() { }

    public ProjectRoot(string path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            try
            {
                FullPath = Path.GetFullPath(path);
            }
            catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
            {
                FullPath = string.Empty;
            }
        }
    }

    public string FullPath { get; } = string.Empty;
}
