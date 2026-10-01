using Oib.Vezba04.Domain.Modeling;

namespace Oib.Vezba04.ConsoleUi;

public static class DemoData
{
    private static readonly Asset PersonalData = new("Lični podaci", BusinessImpact: 5);
    private static readonly Asset InternalReports = new("Interni izveštaji", BusinessImpact: 2);

    public static IReadOnlyList<ThreatScenario> Scenarios { get; } =
    [
        new ThreatScenario(
            "Zloupotreba internog pregleda izveštaja",
            new DataFlow("Pregled u internom sistemu", InternalReports, CrossesTrustBoundary: false),
            Likelihood: 2,
            "Evidencija pregleda i periodična revizija uloga"),
        new ThreatScenario(
            "Neovlašćen primalac izvoza",
            new DataFlow("Izvoz iz internog sistema", PersonalData, CrossesTrustBoundary: true),
            Likelihood: 3,
            "Potpisan zahtev i enkripcija primaoca"),
        new ThreatScenario(
            "Scenario bez procenjene verovatnoće",
            new DataFlow("Uvoz iz spoljnog sistema", PersonalData, CrossesTrustBoundary: true),
            Likelihood: 0,
            "Validacija ulaza na granici poverenja")
    ];
}
