namespace Oib.Vezba01;

public sealed class RoleCatalog
{
    private readonly IReadOnlyDictionary<string, IReadOnlySet<string>> _permissions =
        new Dictionary<string, IReadOnlySet<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Operator"] = new HashSet<string>(["reports:view"], StringComparer.OrdinalIgnoreCase),
            ["SecurityAdmin"] = new HashSet<string>(["reports:view", "reports:export"], StringComparer.OrdinalIgnoreCase),
        };

    public bool Grants(string role, string permission) =>
        _permissions.TryGetValue(role, out var granted) && granted.Contains(permission);
}

