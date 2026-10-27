using System.Diagnostics;
using EquipmentReservation.Mcp.Interfaces;
using EquipmentReservation.Mcp.Models;
using EquipmentReservation.Mcp.Workspace;

namespace EquipmentReservation.Mcp.Processes;

public sealed class ProjectCommandRunner : IProjectCommandRunner
{
    private readonly ProjectRoot _root;

    public ProjectCommandRunner(ProjectRoot root)
    {
        _root = root;
    }

    private const int MaxOutputCharacters = 8_000;

    public async Task<ProcessResult> RunAsync(ProjectCommand command, CancellationToken cancellationToken)
    {
        if (command is null || _root.FullPath == string.Empty || cancellationToken.IsCancellationRequested)
            return new ProcessResult(-1, string.Empty, "InvalidCommandOrWorkspace");

        try
        {
            using var process = new Process { StartInfo = CreateStartInfo(command) };
            if (!process.Start())
                return new ProcessResult(-1, string.Empty, "ProcessCouldNotStart");

            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();
            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);

                await Task.WhenAll(outputTask, errorTask);
                return new ProcessResult(-1, Limit(await outputTask), "CommandCancelled");
            }

            return new ProcessResult(process.ExitCode, Limit(await outputTask), Limit(await errorTask));
        }
        catch (Exception exception) when (exception is not OutOfMemoryException)
        {
            return new ProcessResult(-1, string.Empty, $"CommandFailed: {exception.Message}");
        }
    }

    private ProcessStartInfo CreateStartInfo(ProjectCommand command)
    {
        var startInfo = new ProcessStartInfo(command.FileName)
        {
            WorkingDirectory = _root.FullPath,
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
        value.Length <= MaxOutputCharacters ? value : value[..MaxOutputCharacters] + "\n... output truncated ...";
}
