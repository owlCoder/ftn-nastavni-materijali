using Presentation.Interfaces;
namespace Presentation.Console;

/// <summary>Podrazumevana implementacija <see cref="IConsoleIO"/> nad sistemskom konzolom.</summary>
public sealed class SystemConsoleIO : IConsoleIO
{
    public void Write(string text) => System.Console.Write(text);

    public void WriteLine(string text) => System.Console.WriteLine(text);

    public void WriteLine() => System.Console.WriteLine();

    public string ReadLine() => System.Console.ReadLine() ?? string.Empty;
}
