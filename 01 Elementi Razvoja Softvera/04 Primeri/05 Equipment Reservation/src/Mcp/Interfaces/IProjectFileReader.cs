using EquipmentReservation.Mcp.Models;

namespace EquipmentReservation.Mcp.Interfaces;

public interface IProjectFileReader
{
    ProjectFileReadResult ReadText(string relativePath);
}
