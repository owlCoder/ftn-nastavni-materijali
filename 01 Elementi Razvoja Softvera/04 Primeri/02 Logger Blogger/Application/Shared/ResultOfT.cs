namespace Shared;

public sealed class Result<TValue>
{
    private Result(bool isSuccess, TValue value, AppError error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TValue Value { get; }
    public AppError Error { get; }

    public static Result<TValue> Success(TValue value) => new(true, value, new AppError());

    // Za neuspeh se eksplicitno prosleđuje bezbedan prazan objekat odgovarajućeg tipa.
    public static Result<TValue> Failure(string code, string message, TValue fallback) => new(false, fallback, new AppError(code, message));
}
