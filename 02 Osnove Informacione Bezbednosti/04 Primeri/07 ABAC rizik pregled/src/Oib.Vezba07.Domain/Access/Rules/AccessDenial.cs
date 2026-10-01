namespace Oib.Vezba07.Domain.Access.Rules;

public sealed record AccessDenial(
    string Code,
    string Reason,
    TimeSpan ReviewIn);
