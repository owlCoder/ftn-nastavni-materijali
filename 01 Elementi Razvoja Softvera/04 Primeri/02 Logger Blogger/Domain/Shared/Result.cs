namespace Shared;

public sealed record AppError(string Code, string Message);

public sealed class Result
{
    private Result(bool isSuccess, AppError error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public AppError Error { get; }

    public static Result Success() => new(true, new AppError(string.Empty, string.Empty));

    public static Result Failure(string code, string message) => new(false, new AppError(code, message));
}

public sealed class Result<TValue>
{
    private Result(bool isSuccess, TValue? value, AppError error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public TValue? Value { get; }

    public AppError Error { get; }

    public static Result<TValue> Success(TValue value) => new(true, value, new AppError(string.Empty, string.Empty));

    public static Result<TValue> Failure(string code, string message) => new(false, default, new AppError(code, message));
}

public sealed record Unit;
