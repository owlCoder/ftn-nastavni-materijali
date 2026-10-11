using Domain;
using Domain.Results;
using Application.Stock.Interfaces;

namespace BusinessImpl.Stock
{
    public class CommonStockWithdrawHandler : IStockWithdrawHandler
    {
        public IStockWithdrawHandler LocalStockWithdrawHandler { private get; set; }
        public IStockWithdrawHandler SupplierStockWithdrawHandler { private get; set; }
        public IStockAddHandler LocalStockAddHandler { private get; set; }
        public IStockAddHandler SupplierStockAddHandler { private get; set; }

        public CommonStockWithdrawHandler(
            IStockWithdrawHandler localStockWithdrawHandler,
            IStockWithdrawHandler supplierStockWithdrawHandler,
            IStockAddHandler localStockAddHandler,
            IStockAddHandler supplierStockAddHandler)
        {
            LocalStockWithdrawHandler = localStockWithdrawHandler;
            SupplierStockWithdrawHandler = supplierStockWithdrawHandler;
            LocalStockAddHandler = localStockAddHandler;
            SupplierStockAddHandler = supplierStockAddHandler;
        }

        public void WithdrawProduct(ProductEntity product, float quantity, LocalStockEntity localStock, SupplierStockEntity supplierStock, out float missingQuantity, Result result)
        {
            missingQuantity = 0;
            LocalStockWithdrawHandler.WithdrawProduct(product, quantity, localStock, supplierStock, out float localStockMissingQuantity, result);
            if (result.Success && localStockMissingQuantity > 0)
            {
                SupplierStockWithdrawHandler.WithdrawProduct(product, localStockMissingQuantity, localStock, supplierStock, out missingQuantity, result);
                if (!result.Success)
                {
                    SupplierStockAddHandler.AddProduct(product, supplierStock, localStockMissingQuantity - missingQuantity);
                    LocalStockAddHandler.AddProduct(product, localStock, quantity - localStockMissingQuantity);
                }
            }
        }

    }
}
