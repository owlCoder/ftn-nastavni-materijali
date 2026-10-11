using Domain;
using Application.Stock.Interfaces;
using Application.Supplier.Interfaces;
using System;

namespace BusinessImpl.Stock
{
    public class SupplierStockChecker : IStockChecker
    {
        private readonly ISupplierStockService service;
        public SupplierStockChecker(ISupplierStockService service) {
            this.service = service;
        }
        public float GetQuantity(ProductEntity product, StockEntity stock)
        {
            if (!(stock is SupplierStockEntity) )
            {
                throw new ArgumentException();
            }
            return service.GetQuantity(((SupplierStockEntity)stock).WebServiceURL, product.ProductId);
        }
    }
}
