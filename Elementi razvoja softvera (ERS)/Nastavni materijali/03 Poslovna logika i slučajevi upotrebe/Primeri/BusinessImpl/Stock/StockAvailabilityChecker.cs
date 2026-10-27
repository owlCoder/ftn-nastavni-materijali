using Domain;
using Domain.Results;
using Application.Stock.Interfaces;

namespace BusinessImpl.Stock
{
    public class StockAvailabilityChecker : IStockAvailabilityChecker
    {
        public IStockChecker LocalStockChecker { private get; set; }
        public IStockChecker SupplierStockChecker { private get; set; }

        public StockAvailabilityChecker(IStockChecker localStockChecker, IStockChecker supplierStockChecker)
        {
            LocalStockChecker = localStockChecker;
            SupplierStockChecker = supplierStockChecker;
        }

        public bool CheckAvailability(ProductEntity product, LocalStockEntity localStock, SupplierStockEntity supplierStock, float quantity, Result result)
        {
            float missingQuantity1 = quantity - LocalStockChecker.GetQuantity(product, localStock);
            if (missingQuantity1 > 0)
            {
                float misslingQuantity2 = missingQuantity1 - SupplierStockChecker.GetQuantity(product, supplierStock);
                if (misslingQuantity2 > 0)
                {
                    result.Log(LogLevel.Error, "Missing quantity: " + misslingQuantity2);
                    return false;
                }
            }
            return true;
        }
    }
}
