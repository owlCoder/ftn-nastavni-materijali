using EquipmentReservation.Mcp.Models;

namespace EquipmentReservation.Mcp.Interfaces;

public interface IProjectCommandRunner
{
    Task<ProcessResult> RunAsync(ProjectCommand command, CancellationToken cancellationToken);
}
