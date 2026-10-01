namespace EquipmentReservation.Mcp.Processes;

public sealed class ProjectCommand
{
    private ProjectCommand(string fileName, params string[] arguments)
    {
        FileName = fileName;
        Arguments = arguments;
    }

    public string FileName { get; }

    public IReadOnlyList<string> Arguments { get; }

    public static ProjectCommand GitDiff { get; } = new("git", "diff", "--", ".");

    public static ProjectCommand RunUnitTests { get; } = new(
        "dotnet",
        "test",
        "EquipmentReservation.sln",
        "--nologo",
        "--verbosity",
        "minimal");
}
