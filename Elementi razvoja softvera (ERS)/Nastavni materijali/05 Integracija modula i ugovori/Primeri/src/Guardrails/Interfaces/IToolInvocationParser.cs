using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Interfaces;

public interface IToolInvocationParser
{
    ToolInvocation Parse(string json);
}
