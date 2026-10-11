using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Shared;

namespace Infrastructure.Services;

/// <summary>Servis za loggere: prima evidencije, evidentira ishod kroz logger.</summary>
public sealed class EvidencijaServis : IZapisiServis
{
    private readonly IZapisiRepozitorijum _zapisiRepozitorijum;
    private readonly ILoggerServis _logger;

    public EvidencijaServis(IZapisiRepozitorijum zapisiRepozitorijum, ILoggerServis logger)
    {
        _zapisiRepozitorijum = zapisiRepozitorijum;
        _logger = logger;
    }

    public Result<ZapisNaSajtu> Objavi(ZapisNaSajtu zapis)
    {
        if (zapis is not Evidencija)
        {
            _logger.Loguj("Zapis u evidenciji nije kreiran: pogrešan tip zapisa.");
            return Result<ZapisNaSajtu>.Failure("evidencija.pogresan_tip", "Zapis nije evidencija.");
        }

        Result<ZapisNaSajtu> dodat = _zapisiRepozitorijum.Dodaj(zapis);
        if (dodat.IsFailure)
        {
            _logger.Loguj($"Novi zapis u evidenciji nije dodat na veb stranicu: {dodat.Error.Message}");
            return dodat;
        }

        _logger.Loguj("Novi zapis u evidenciji je dodat na veb stranicu.");
        return dodat;
    }

    public Result<IReadOnlyList<ZapisNaSajtu>> PregledZapisa()
    {
        return _zapisiRepozitorijum.VratiSve();
    }
}
