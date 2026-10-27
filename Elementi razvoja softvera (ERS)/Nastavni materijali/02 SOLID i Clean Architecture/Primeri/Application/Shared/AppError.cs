namespace Shared;

public sealed class AppError
{
    public AppError() { }

    public AppError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; } = string.Empty;
    public string Message { get; } = string.Empty;
}
