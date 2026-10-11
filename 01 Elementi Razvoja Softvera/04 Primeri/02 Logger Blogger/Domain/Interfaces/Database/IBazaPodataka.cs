namespace Domain.Interfaces.Database;

/// <summary>
/// Skladište podataka iza repozitorijuma. Drži tabele u memoriji i ume da ih
/// perzistira (npr. u JSON). Repozitorijumi zavise samo od ovog ugovora.
/// </summary>
public interface IBazaPodataka
{
    TabeleBazaPodataka Tabele { get; }

    bool SacuvajPromene();
}
