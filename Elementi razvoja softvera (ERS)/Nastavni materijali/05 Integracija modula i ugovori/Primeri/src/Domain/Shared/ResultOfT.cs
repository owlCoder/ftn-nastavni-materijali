namespace EquipmentReservation.Domain.Shared;

public sealed class Result<T>
{
    private Result(bool success, T value, string error)
    {
        Success = success;
        Value = value;
        Error = error;
    }

    public bool Success { get; }
    public T Value { get; }
    public string Error { get; }

    public static Result<T> Ok(T value) => new(true, value, string.Empty);
    public static Result<T> Fail(string error, T fallback) => new(false, fallback, string.IsNullOrWhiteSpace(error) ? "UnknownError" : error);
}
