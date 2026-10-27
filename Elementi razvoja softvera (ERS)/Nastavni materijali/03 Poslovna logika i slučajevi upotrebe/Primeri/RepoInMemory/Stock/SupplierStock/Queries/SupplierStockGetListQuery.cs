using Application.Stock.SupplierStock.Queries;
using RepoInMemory.Common.Queries;
using Domain;

namespace RepoInMemory.Stock.SupplierStock.Queries
{
    public class SupplierStockGetListQuery : BaseGetListQuery<SupplierStockEntity>, ISupplierStockGetListQuery { }
}
