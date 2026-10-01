using System.Diagnostics;
using EquipmentReservation.Mcp.Workspace;

namespace EquipmentReservation.Mcp.Processes;

public sealed class ProjectCommandRunner(ProjectRoot root) : IProjectCommandRunner
{
    private const int MaxOutputCharacters = 8_000;

    public async Task<ProcessResult> RunAsync(
        ProjectCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        using var process = Process.Start(CreateStartInfo(command))
            ?? throw new InvalidOperationException($"Could not start '{command.FileName}'.");

        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);

            throw;
        }

        return new ProcessResult(
            process.ExitCode,
            Limit(await outputTask),
            Limit(await errorTask));
    }

    private ProcessStartInfo CreateStartInfo(ProjectCommand command)
    {
        var startInfo = new ProcessStartInfo(command.FileName)
        {
            WorkingDirectory = root.FullPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in command.Arguments)
            startInfo.ArgumentList.Add(argument);

        return startInfo;
    }

    private static string Limit(string value) =>
        value.Length <= MaxOutputCharacters
            ? value
            : value[..MaxOutputCharacters] + "\n... output truncated ...";
}
