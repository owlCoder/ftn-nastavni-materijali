using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stock.LocalStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.LocalStock.Commands
{
    public class LocalStockInsertOrUpdateCommand : BaseInsertOrUpdateCommand<LocalStockEntity>, ILocalStockInsertOrUpdateCommand
    {
        public LocalStockInsertOrUpdateCommand(IFindByIdQuery<LocalStockEntity> findByIdQuery, IInsertCommand<LocalStockEntity> insertCommand, IUpdateCommand<LocalStockEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
