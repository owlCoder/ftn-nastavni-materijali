using Application.Stock.SupplierStock.Queries;
using Domain;
using RepoInMemory.Common.DB;
using System.Linq;

namespace RepoInMemory.Stock.SupplierStock.Queries
{
    public class SupplierStockGetDefaultSupplierStockQuery : ISupplierStockGetDefaultSupplierStockQuery
    {
        public SupplierStockEntity GetDefaultSupplierStock(SupplierEntity supplier)
        {
            return InMemoryDatabase.Instance.SupplierStocks.FirstOrDefault(t => t.Value.Supplier.SupplierId == supplier.SupplierId && t.Value.IsDefault).Value;
        }
    }
}
