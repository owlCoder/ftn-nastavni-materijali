using Application.Common.Factory;
using Application.Product.Commands;
using Application.Stock.LocalStock.Commands;
using Domain;
using Domain.Results;
using Application.Stock.Interfaces;
using System.Linq;

namespace BusinessImpl.Stock
{
    public class LocalStockWithdrawHandler : IStockWithdrawHandler
    {
        public ILocalStockUpdateCommand StockUpdateCommand { private get; set; }
        public IProductInsertIfNotExistsCommand ProductInsertIfNotExistsCommand { private get; set; }


        public LocalStockWithdrawHandler(IRepoFactory repoFactory) : base()
        {
            StockUpdateCommand = repoFactory.LocalStockUpdateCommand;
            ProductInsertIfNotExistsCommand = repoFactory.ProductInsertIfNotExistsCommand;
        }



        public void WithdrawProduct(ProductEntity product, float quantity, LocalStockEntity localStock, SupplierStockEntity supplierStock, out float missingQuantity, Result result)
        {
            ProductInsertIfNotExistsCommand.InsertIfNotExists(product);
            WithdrawProductUpdateStock(product, quantity, localStock, out missingQuantity);
            if (result.Success)
            {
                if (!StockUpdateCommand.Update(localStock))
                {
                    result.Log(LogLevel.Error, "Unsuccesful stock update");
                }
            }
        }

        protected bool WithdrawProductUpdateStock(ProductEntity product, float quantity, LocalStockEntity localStock, out float missingQuantity)
        {
            ProductStock ps = localStock.ProductStocks.Where(t => t.Product.ProductId == product.ProductId && t.Stock.StockId == localStock.StockId).FirstOrDefault();
            if (ps != null)
            {
                return UpdateExistingProductOnStock(ps, quantity, out missingQuantity);
            }
            else
            {
                return AddMissingProductOnStock(product, quantity, localStock, out missingQuantity);
            }
        }

        private bool UpdateExistingProductOnStock(ProductStock ps, float quantity, out float missingQuantity)
        {
            missingQuantity = quantity - ps.Quantity;
            if (missingQuantity > 0)
            {
                ps.Quantity = 0;
                return false;
            }
            missingQuantity = 0;
            ps.Quantity -= quantity;
            return true;
        }

        private bool AddMissingProductOnStock(ProductEntity product, float quantity, LocalStockEntity localStock, out float missingQuantity)
        {
            ProductStock ps = new ProductStock(product, localStock, 0);
            localStock.ProductStocks.Add(ps);
            missingQuantity = quantity;
            return false;
        }
    }
}
