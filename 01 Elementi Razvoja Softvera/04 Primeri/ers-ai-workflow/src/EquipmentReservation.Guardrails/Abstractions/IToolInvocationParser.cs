using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Abstractions;

public interface IToolInvocationParser
{
    ToolInvocation Parse(string json);
}
