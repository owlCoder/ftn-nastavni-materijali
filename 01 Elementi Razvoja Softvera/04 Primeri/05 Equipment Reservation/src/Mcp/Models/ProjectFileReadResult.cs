namespace EquipmentReservation.Mcp.Models;

public sealed class ProjectFileReadResult
{
    public bool Success { get; private init; }
    public string Content { get; private init; } = string.Empty;
    public string Error { get; private init; } = string.Empty;

    public static ProjectFileReadResult Ok(string content) => new() { Success = true, Content = content };
    public static ProjectFileReadResult Fail(string error) => new() { Error = error };
}
