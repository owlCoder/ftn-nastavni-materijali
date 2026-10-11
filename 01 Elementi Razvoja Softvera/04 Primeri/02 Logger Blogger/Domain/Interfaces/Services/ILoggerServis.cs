namespace Domain.Interfaces.Services;

public interface ILoggerServis
{
    /// <summary>Upisuje poruku evidencije u log (npr. log.txt).</summary>
    /// <returns>True ako je poruka uspešno zabeležena, u suprotnom false.</returns>
    bool Loguj(string poruka);
}
