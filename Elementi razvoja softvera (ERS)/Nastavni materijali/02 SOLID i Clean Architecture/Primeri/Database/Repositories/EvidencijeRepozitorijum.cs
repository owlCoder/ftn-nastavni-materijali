using Database.Interfaces;
using Application.Interfaces.Repositories;
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
            return Result<ZapisNaSajtu>.Failure("evidencija.pogresan_tip", "Zapis nije evidencija.", new Evidencija());
        }

        if (!_bazaPodataka.IsReady)
            return Result<ZapisNaSajtu>.Failure("baza.nedostupna", "Skladište nije spremno.", new Evidencija());

        _bazaPodataka.Tabele.Evidencije.Add(evidencija);
        if (!_bazaPodataka.SacuvajPromene())
        {
            _bazaPodataka.Tabele.Evidencije.Remove(evidencija);
            return Result<ZapisNaSajtu>.Failure("baza.upis", "Upis nije uspeo.", new Evidencija());
        }
        return Result<ZapisNaSajtu>.Success(evidencija);
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> VratiSve()
    {
        if (!_bazaPodataka.IsReady)
            return Result<IReadOnlyList<ZapisNaSajtu>>.Failure("baza.nedostupna", "Skladište nije spremno.", new List<ZapisNaSajtu>());

        return Result<IReadOnlyList<ZapisNaSajtu>>.Success(_bazaPodataka.Tabele.Evidencije.ToList<ZapisNaSajtu>());
    }
}
