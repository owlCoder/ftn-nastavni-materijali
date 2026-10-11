using Domain.Interfaces.Database;

namespace Database.Connection.Providers;

/// <summary>Skladište koje drži podatke samo u memoriji; ne perzistira ništa.</summary>
public sealed class InMemoryBazaPodataka : IBazaPodataka
{
    public TabeleBazaPodataka Tabele { get; } = new();

    public bool SacuvajPromene() => true;
}
