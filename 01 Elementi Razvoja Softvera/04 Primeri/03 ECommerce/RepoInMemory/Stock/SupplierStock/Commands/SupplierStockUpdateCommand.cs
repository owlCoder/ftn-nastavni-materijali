using Application.Stock.SupplierStock.Commands;
using RepoInMemory.Common.Commands;
using Domain;

namespace RepoInMemory.Stock.SupplierStock.Commands
{
    public class SupplierStockUpdateCommand : BaseUpdateCommand<SupplierStockEntity>, ISupplierStockUpdateCommand { }
}
