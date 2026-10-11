using Domain.Interfaces.Database;
using Domain.Interfaces.Repositories;
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
            return Result<ZapisNaSajtu>.Failure("objava.pogresan_tip", "Zapis nije objava.");
        }

        _bazaPodataka.Tabele.Objave.Add(objava);
        _bazaPodataka.SacuvajPromene();
        return Result<ZapisNaSajtu>.Success(objava);
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> VratiSve()
    {
        return Result<IReadOnlyList<ZapisNaSajtu>>.Success(_bazaPodataka.Tabele.Objave.ToList<ZapisNaSajtu>());
    }
}
