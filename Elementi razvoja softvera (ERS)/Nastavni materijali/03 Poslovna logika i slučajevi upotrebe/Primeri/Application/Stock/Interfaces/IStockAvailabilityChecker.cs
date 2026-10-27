using Domain;
using Domain.Results;

namespace Application.Stock.Interfaces
{
    public interface IStockAvailabilityChecker
    {
        bool CheckAvailability(ProductEntity product, LocalStockEntity localStock, SupplierStockEntity supplierStock, float quantity, Result result);
    }
}
