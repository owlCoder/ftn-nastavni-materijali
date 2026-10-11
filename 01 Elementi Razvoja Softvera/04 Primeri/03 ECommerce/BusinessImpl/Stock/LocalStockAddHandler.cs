using Application.Common.Factory;
using Application.Product.Commands;
using Application.Stock.LocalStock.Commands;
using Domain;
using Application.Stock.Interfaces;
using System;

namespace BusinessImpl.Stock
{
    public class LocalStockAddHandler : BaseStockAddHandler, IStockAddHandler
    {
        public IProductInsertIfNotExistsCommand ProductInsertIfNotExistsCommand { private get; set; }
        public ILocalStockUpdateCommand StockUpdateCommand { private get; set; }

        public LocalStockAddHandler(IRepoFactory repoFactory) : base()
        {
            StockUpdateCommand = repoFactory.LocalStockUpdateCommand;
            ProductInsertIfNotExistsCommand = repoFactory.ProductInsertIfNotExistsCommand;
        }

        public void AddProduct(ProductEntity product, StockEntity stock, float productQuantity)
        {
            if (!(stock is LocalStockEntity))
            {
                throw new ArgumentException("Invalid type");
            }
            ProductInsertIfNotExistsCommand.InsertIfNotExists(product);
            AddProductUpdateStock(product, stock, productQuantity);
            StockUpdateCommand.Update((LocalStockEntity)stock);
        }
    }
}
