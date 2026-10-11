using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Shared;

namespace Infrastructure.Services;

/// <summary>Servis za bloggere: prima objave, evidentira ishod kroz logger.</summary>
public sealed class BlogServis : IZapisiServis
{
    private readonly IZapisiRepozitorijum _zapisiRepozitorijum;
    private readonly ILoggerServis _logger;

    public BlogServis(IZapisiRepozitorijum zapisiRepozitorijum, ILoggerServis logger)
    {
        _zapisiRepozitorijum = zapisiRepozitorijum;
        _logger = logger;
    }

    public Result<ZapisNaSajtu> Objavi(ZapisNaSajtu zapis)
    {
        if (zapis is not Objava)
        {
            _logger.Loguj("Objava nije kreirana: pogrešan tip zapisa.");
            return Result<ZapisNaSajtu>.Failure("blog.pogresan_tip", "Zapis nije objava.");
        }

        Result<ZapisNaSajtu> dodat = _zapisiRepozitorijum.Dodaj(zapis);
        if (dodat.IsFailure)
        {
            _logger.Loguj($"Nova objava nije dodata na veb stranicu: {dodat.Error.Message}");
            return dodat;
        }

        _logger.Loguj("Nova objava je dodata na veb stranicu.");
        return dodat;
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> PregledZapisa()
    {
        return _zapisiRepozitorijum.VratiSve();
    }
}
