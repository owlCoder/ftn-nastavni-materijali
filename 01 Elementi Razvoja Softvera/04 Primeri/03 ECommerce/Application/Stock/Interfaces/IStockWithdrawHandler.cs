using Domain;
using Domain.Results;

namespace Application.Stock.Interfaces
{
    public interface IStockWithdrawHandler
    {
        void WithdrawProduct(ProductEntity product, float quantity, LocalStockEntity localStock, SupplierStockEntity supplierStock, out float missingQuantity, Result result);

    }
}
