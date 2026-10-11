using Database.Models;

namespace Database.Interfaces;

public interface IBazaPodataka
{
    TabeleBazaPodataka Tabele { get; }
    bool IsReady { get; }
    bool SacuvajPromene();
}
