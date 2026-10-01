namespace EquipmentReservation.Mcp.Processes;

public interface IProjectCommandRunner
{
    Task<ProcessResult> RunAsync(
        ProjectCommand command,
        CancellationToken cancellationToken);
}
