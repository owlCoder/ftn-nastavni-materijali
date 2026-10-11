namespace Presentation.Console;

/// <summary>
/// Apstrakcija nad ulazom/izlazom konzole. Meni zavisi od ovog ugovora umesto
/// direktno od <see cref="System.Console"/>, pa je logika menija testabilna i odvojena
/// od konkretnog I/O kanala (DIP).
/// </summary>
public interface IConsoleIO
{
    void Write(string text);

    void WriteLine(string text);

    void WriteLine();

    string ReadLine();
}
