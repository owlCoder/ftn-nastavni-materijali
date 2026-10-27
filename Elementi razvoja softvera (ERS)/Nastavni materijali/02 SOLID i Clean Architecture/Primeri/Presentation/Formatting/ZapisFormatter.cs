using System.Globalization;
using Domain.Models;

namespace Presentation.Formatting;

public static class ZapisFormatter
{
    public static string Format(ZapisNaSajtu zapis)
    {
        string opis = Skrati(zapis.Opis, 28);
        string datum = zapis.Postavljeno.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

        return zapis switch
        {
            Objava objava => string.Format(CultureInfo.InvariantCulture,
                "| {0,-8} | {1,-15} | {2,-20} | {3,-30} | {4,-20} |",
                "#" + objava.TipObjave, Skrati(objava.Naslov, 13), objava.Autor.ImePrezime, opis, datum),
            Evidencija evidencija => string.Format(CultureInfo.InvariantCulture,
                "| {0,-12} | {1,-20} | {2,-30} | {3,-20} |",
                "Log_" + evidencija.TipEvidencije, evidencija.Autor.ImePrezime, opis, datum),
            _ => string.Format(CultureInfo.InvariantCulture, "| {0,-20} | {1,-30} | {2,-20} |",
                zapis.Autor.ImePrezime, opis, datum)
        };
    }

    private static string Skrati(string tekst, int duzina) => tekst.Length > duzina ? tekst[..(duzina - 3)] + "..." : tekst;
}
