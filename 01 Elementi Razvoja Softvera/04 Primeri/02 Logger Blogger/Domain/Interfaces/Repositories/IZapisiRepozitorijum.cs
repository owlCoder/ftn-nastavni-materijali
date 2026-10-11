using Domain.Models;
using Shared;

namespace Domain.Interfaces.Repositories;

/// <summary>
/// Repozitorijum nad zapisima na sajtu. Konkretne implementacije rade nad jednom
/// vrstom zapisa (objave ili evidencije), ali dele isti ugovor.
/// </summary>
public interface IZapisiRepozitorijum
{
    Result<ZapisNaSajtu> Dodaj(ZapisNaSajtu zapis);

    Result<IReadOnlyList<ZapisNaSajtu>> VratiSve();
}
