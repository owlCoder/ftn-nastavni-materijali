using System.Text.Json;
using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;

namespace EquipmentReservation.Guardrails.Parsing;

public sealed class HookInputParser : IToolInvocationParser
{
    public ToolInvocation Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        var toolName = TryRead(root, "tool_name");
        var command = TryReadNested(root, "tool_input", "command");
        var filePath = TryReadNested(root, "tool_input", "file_path");

        return new ToolInvocation(toolName, command, filePath);
    }

    private static string? TryRead(JsonElement element, string property) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(property, out var value) &&
        value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static string? TryReadNested(
        JsonElement element,
        string parent,
        string property) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(parent, out var parentValue)
            ? TryRead(parentValue, property)
            : null;
}
