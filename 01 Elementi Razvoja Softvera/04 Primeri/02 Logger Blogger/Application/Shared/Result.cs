namespace Shared;

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

    public static Result Success() => new(true, new AppError());
    public static Result Failure(string code, string message) => new(false, new AppError(code, message));
}
