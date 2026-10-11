using System.Text.Json;
using EquipmentReservation.Guardrails.Interfaces;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Parsing;

public sealed class HookInputParser : IToolInvocationParser
{
    public ToolInvocation Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return Invalid();

        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("tool_name", out var name) ||
                name.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(name.GetString()))
                return Invalid();

            if (!root.TryGetProperty("tool_input", out var input) || input.ValueKind != JsonValueKind.Object)
                return Invalid();

            if (!ValidProperty(input, "command") || !ValidProperty(input, "file_path"))
                return Invalid();

            return new ToolInvocation(name.GetString(), ReadText(input, "command"), ReadText(input, "file_path"));
        }
        catch (JsonException)
        {
            return Invalid();
        }
    }

    private static ToolInvocation Invalid() => new(string.Empty, string.Empty, string.Empty, valid: false);

    private static bool ValidProperty(JsonElement element, string property) =>
        !element.TryGetProperty(property, out var value) || value.ValueKind is JsonValueKind.Null or JsonValueKind.String;

    private static string ReadText(JsonElement element, string property) =>
        element.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString() ?? string.Empty : string.Empty;
}
