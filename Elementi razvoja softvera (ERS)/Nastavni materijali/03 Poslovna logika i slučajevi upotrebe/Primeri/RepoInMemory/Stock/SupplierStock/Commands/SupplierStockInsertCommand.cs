using Application.Stock.SupplierStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.SupplierStock.Commands
{
    public class SupplierStockInsertCommand : BaseInsertCommand<SupplierStockEntity>, ISupplierStockInsertCommand
    {
    }
}
