namespace Odp.Vezba05.Domain.Shared;

public sealed record Result(bool Success, string? Error)
{
    public static Result Ok() => new(true, null);

    public static Result Fail(string error) => new(false, error);
}
