namespace EquipmentReservation.Mcp.Models;

public sealed class ProjectCommand
{
    public ProjectCommand() { }

    private ProjectCommand(string fileName, params string[] arguments)
    {
        FileName = fileName;
        Arguments = arguments;
    }

    public string FileName { get; } = string.Empty;

    public IReadOnlyList<string> Arguments { get; } = Array.Empty<string>();

    public static ProjectCommand GitDiff { get; } = new("git", "diff", "--", ".");

    public static ProjectCommand RunUnitTests { get; } = new("dotnet", "test", "EquipmentReservation.sln", "--nologo", "--verbosity", "minimal");
}
