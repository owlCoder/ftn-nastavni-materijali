namespace Presentation.TekstGenerator;

/// <summary>Pomoćnik koji proizvodi nasumičan tekst za demo objave i evidencije.</summary>
public static class NasumicanTekst
{
    private static readonly Random Random = new();

    private static readonly string[] Opisi =
    [
        "Ovo je divan dan za istraživanje prirode. Sunce sija, a vazduh je svež.",
        "U ovoj priči, junak se suočava sa izazovima. Njegova hrabrost će biti stavljena na probu.",
        "Na obali mora, talasi šumore dok se vetar lagano igra sa granama. Udaljeni horizont poziva na avanturu.",
        "Zima je došla sa svojim čarima. Sneg prekriva sve, a deca se igraju u snežnim grudvama."
    ];

    private static readonly string[] Naslovi =
    [
        "Putovanje kroz čarobni svet",
        "Hrabrost u teškim vremenima",
        "Priče sa obale mora",
        "Zimska čarolija"
    ];

    private static readonly string[] LogPoruke =
    [
        "Uspešno kreiran objekat.",
        "Servis je u ispadu.",
        "Zapis uspešno sačuvan.",
        "Greška prilikom obrade zahteva.",
        "Operacija je uspešna.",
        "Neuspešno povezivanje sa serverom.",
        "Podaci su uspešno učitani.",
        "Nevalidan unos podataka."
    ];

    public static string GetNasumicanOpis() => Opisi[Random.Next(Opisi.Length)];

    public static string GetNasumicanNaslov() => Naslovi[Random.Next(Naslovi.Length)];

    public static string GetNasumicnaLogPoruka() => LogPoruke[Random.Next(LogPoruke.Length)];
}
