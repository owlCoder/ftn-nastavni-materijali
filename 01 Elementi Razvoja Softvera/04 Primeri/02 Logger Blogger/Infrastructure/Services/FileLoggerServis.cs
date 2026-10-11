using System.Globalization;
using Application.Interfaces.Services;

namespace Infrastructure.Services;

public sealed class FileLoggerServis : ILoggerServis
{
    private readonly string _putanja;

    public FileLoggerServis(string putanja = "log.txt")
    {
        _putanja = putanja;
    }

    public bool Loguj(string poruka)
    {
        try
        {
            using StreamWriter sw = new(_putanja, append: true);
            sw.Write($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)}]: {poruka}\n");
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}
