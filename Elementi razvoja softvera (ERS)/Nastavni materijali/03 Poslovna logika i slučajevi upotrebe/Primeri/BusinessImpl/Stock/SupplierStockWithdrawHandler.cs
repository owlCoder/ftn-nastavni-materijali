using Domain;
using Domain.Results;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;

namespace BusinessImpl.Stock
{
    public class SupplierStockWithdrawHandler : IStockWithdrawHandler
    {
        private readonly ISupplierStockService supplierStockService;
        public SupplierStockWithdrawHandler(ISupplierStockService supplierStockService) : base()
        {
            this.supplierStockService = supplierStockService;
        }

        public void WithdrawProduct(ProductEntity product, float quantity, LocalStockEntity localStock, SupplierStockEntity supplierStock, out float missingQuantity, Result result)
        {
            float quantityOnStock = supplierStockService.GetQuantity((supplierStock).WebServiceURL, product.ProductId);
            missingQuantity = quantity - quantityOnStock;
            if (missingQuantity > 0)
            {
                result.Log(LogLevel.Error, "Missing products on supplier stock");
            }
            else if (!supplierStockService.WithdrawProduct((supplierStock).WebServiceURL, product.ProductId, quantityOnStock))
            {
                result.Log(LogLevel.Error, "Missing products on supplier stock");
            }
        }

    }
}
