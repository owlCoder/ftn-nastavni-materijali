using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stock.SupplierStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.SupplierStock.Commands
{
    public class SupplierStockInsertIfNotExistsCommand : BaseInsertIfNotExistsCommand<SupplierStockEntity>, ISupplierStockInsertIfNotExistsCommand
    {
        public SupplierStockInsertIfNotExistsCommand(IFindByIdQuery<SupplierStockEntity> findByIdQuery, IInsertCommand<SupplierStockEntity> insertCommand) : base(findByIdQuery, insertCommand) { }
    }
}
