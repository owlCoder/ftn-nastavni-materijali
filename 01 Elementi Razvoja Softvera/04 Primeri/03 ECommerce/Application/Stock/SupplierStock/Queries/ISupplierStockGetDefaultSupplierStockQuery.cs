using Domain;

namespace Application.Stock.SupplierStock.Queries
{
    public interface ISupplierStockGetDefaultSupplierStockQuery
    {
        public SupplierStockEntity GetDefaultSupplierStock(SupplierEntity supplier);
    }
}
