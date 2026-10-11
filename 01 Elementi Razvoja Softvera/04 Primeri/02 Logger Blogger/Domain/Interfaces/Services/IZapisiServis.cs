using Domain.Models;
using Shared;

namespace Domain.Interfaces.Services;

/// <summary>
/// Servis za objavljivanje i pregled zapisa na sajtu. Konkretne implementacije
/// (blog, evidencija) rade nad odgovarajućom vrstom zapisa.
/// </summary>
public interface IZapisiServis
{
    Result<ZapisNaSajtu> Objavi(ZapisNaSajtu zapis);

    Result<IReadOnlyList<ZapisNaSajtu>> PregledZapisa();
}
