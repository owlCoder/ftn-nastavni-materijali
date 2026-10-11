using Application.Stock.LocalStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.LocalStock.Commands
{
    public class LocalStockInsertCommand : BaseInsertCommand<LocalStockEntity>, ILocalStockInsertCommand
    {
    }
}
