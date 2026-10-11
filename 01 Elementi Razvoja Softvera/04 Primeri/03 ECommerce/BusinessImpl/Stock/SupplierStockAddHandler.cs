using Domain;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;
using System;
using System.Linq;

namespace BusinessImpl.Stock
{
    public class SupplierStockAddHandler : BaseStockAddHandler, IStockAddHandler
    {
        public ISupplierStockService SupplierStockService { get; set; }


        public SupplierStockAddHandler(ISupplierStockService supplierStockService) : base()
        {
            this.SupplierStockService = supplierStockService;
        }

        public void AddProduct(ProductEntity product, StockEntity stock, float productQuantity)
        {
            if (!(stock is SupplierStockEntity)) {
                throw new ArgumentException("Invalid type");
            }
            AddProductUpdateStock(product, stock, productQuantity);
            SupplierStockService.UpdateProduct(((SupplierStockEntity)stock).WebServiceURL, product.ProductId, stock.ProductStocks.Where(t => t.Product.ProductId == product.ProductId).FirstOrDefault().Quantity);
        }
    }
}
