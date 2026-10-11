using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stock.LocalStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.LocalStock.Commands
{
    public class LocalStockInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<LocalStockEntity>, ILocalStockInsertIfNotExistsCommand
    {
        public LocalStockInsertIfNotExistsCommand(IFindByIdQuery<LocalStockEntity> findByIdQuery, IInsertCommand<LocalStockEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
