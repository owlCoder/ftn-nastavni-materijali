using System.Diagnostics;
using System.Text.Json;

namespace EquipmentReservation.Mcp.Workspace;

public sealed class ProjectWorkspace(string rootPath)
{
    private static readonly HashSet<string> AllowedTextExtensions = new(
        StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".md", ".json", ".props", ".sln"
    };

    public string RootPath { get; } = Path.GetFullPath(rootPath);

    public string ReadProjectFile(string relativePath)
    {
        var fullPath = ResolveSafePath(relativePath);
        var extension = Path.GetExtension(fullPath);
        if (!AllowedTextExtensions.Contains(extension))
            throw new InvalidOperationException(
                $"File type '{extension}' is not exposed by the MCP server.");

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Project file was not found.", relativePath);

        return File.ReadAllText(fullPath);
    }

    public string GetStructure()
    {
        var entries = Directory
            .EnumerateFiles(RootPath, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(RootPath, path))
            .Where(path => !ContainsSegment(path, ".git"))
            .Where(path => !ContainsSegment(path, "bin"))
            .Where(path => !ContainsSegment(path, "obj"))
            .Where(path => !Path.GetFileName(path).Equals(".env", StringComparison.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase);

        return string.Join('\n', entries);
    }

    public async Task<ProcessResult> RunFixedCommandAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo(fileName)
        {
            WorkingDirectory = RootPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Could not start '{fileName}'.");

        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);
        return new ProcessResult(
            process.ExitCode,
            Limit(await outputTask),
            Limit(await errorTask));
    }

    public string ToJson(object value) =>
        JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

    private string ResolveSafePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentException("Relative path is required.", nameof(relativePath));

        if (Path.IsPathRooted(relativePath))
            throw new InvalidOperationException("Only project-relative paths are allowed.");

        var combined = Path.GetFullPath(Path.Combine(RootPath, relativePath));
        var rootWithSeparator =
            RootPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

        var pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!combined.StartsWith(rootWithSeparator, pathComparison))
            throw new InvalidOperationException(
                "Path traversal outside the project root is blocked.");

        if (combined.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(segment => segment.Equals(".git", StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("The .git directory is not exposed.");

        if (Path.GetFileName(combined).Equals(".env", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Sensitive files are not exposed.");

        return combined;
    }

    private static bool ContainsSegment(string path, string segment) =>
        path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(item => item.Equals(segment, StringComparison.OrdinalIgnoreCase));

    private static string Limit(string value)
    {
        const int max = 8_000;
        return value.Length <= max
            ? value
            : value[..max] + "\n... output truncated ...";
    }
}
