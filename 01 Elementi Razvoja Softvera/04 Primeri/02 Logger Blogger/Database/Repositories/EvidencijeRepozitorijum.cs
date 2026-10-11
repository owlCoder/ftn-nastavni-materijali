using Domain.Interfaces.Database;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Shared;

namespace Database.Repositories;

public sealed class EvidencijeRepozitorijum : IZapisiRepozitorijum
{
    private readonly IBazaPodataka _bazaPodataka;

    public EvidencijeRepozitorijum(IBazaPodataka bazaPodataka)
    {
        _bazaPodataka = bazaPodataka;
    }

    public Result<ZapisNaSajtu> Dodaj(ZapisNaSajtu zapis)
    {
        if (zapis is not Evidencija evidencija)
        {
            return Result<ZapisNaSajtu>.Failure("evidencija.pogresan_tip", "Zapis nije evidencija.");
        }

        _bazaPodataka.Tabele.Evidencije.Add(evidencija);
        _bazaPodataka.SacuvajPromene();
        return Result<ZapisNaSajtu>.Success(evidencija);
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> VratiSve()
    {
        return Result<IReadOnlyList<ZapisNaSajtu>>.Success(_bazaPodataka.Tabele.Evidencije.ToList<ZapisNaSajtu>());
    }
}
