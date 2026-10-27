using Database.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Models;
using Shared;

namespace Database.Repositories;

public sealed class ObjaveRepozitorijum : IZapisiRepozitorijum
{
    private readonly IBazaPodataka _bazaPodataka;

    public ObjaveRepozitorijum(IBazaPodataka bazaPodataka)
    {
        _bazaPodataka = bazaPodataka;
    }

    public Result<ZapisNaSajtu> Dodaj(ZapisNaSajtu zapis)
    {
        if (zapis is not Objava objava)
        {
            return Result<ZapisNaSajtu>.Failure("objava.pogresan_tip", "Zapis nije objava.", new Objava());
        }

        if (!_bazaPodataka.IsReady)
            return Result<ZapisNaSajtu>.Failure("baza.nedostupna", "Skladište nije spremno.", new Objava());

        _bazaPodataka.Tabele.Objave.Add(objava);
        if (!_bazaPodataka.SacuvajPromene())
        {
            _bazaPodataka.Tabele.Objave.Remove(objava);
            return Result<ZapisNaSajtu>.Failure("baza.upis", "Upis nije uspeo.", new Objava());
        }
        return Result<ZapisNaSajtu>.Success(objava);
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> VratiSve()
    {
        if (!_bazaPodataka.IsReady)
            return Result<IReadOnlyList<ZapisNaSajtu>>.Failure("baza.nedostupna", "Skladište nije spremno.", new List<ZapisNaSajtu>());

        return Result<IReadOnlyList<ZapisNaSajtu>>.Success(_bazaPodataka.Tabele.Objave.ToList<ZapisNaSajtu>());
    }
}
