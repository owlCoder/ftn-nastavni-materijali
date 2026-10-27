using System.Text.Json;

namespace EquipmentReservation.Mcp.Tools.Results;

public static class ToolResultJson
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string Serialize<T>(T result) => JsonSerializer.Serialize(result, Options);
}
