using Application.Stock.LocalStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.LocalStock.Commands
{
    public class LocalStockUpdateCommand : BaseUpdateCommand<LocalStockEntity>, ILocalStockUpdateCommand { }
}
