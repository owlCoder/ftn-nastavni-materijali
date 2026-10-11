using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stock.SupplierStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.SupplierStock.Commands
{
    public class SupplierStockInsertOrUpdateCommand : BaseInsertOrUpdateCommand<SupplierStockEntity>, ISupplierStockInsertOrUpdateCommand
    {
        public SupplierStockInsertOrUpdateCommand(IFindByIdQuery<SupplierStockEntity> findByIdQuery, IInsertCommand<SupplierStockEntity> insertCommand, IUpdateCommand<SupplierStockEntity> updateCommand) : base(findByIdQuery, insertCommand, updateCommand) { }
    }
}
