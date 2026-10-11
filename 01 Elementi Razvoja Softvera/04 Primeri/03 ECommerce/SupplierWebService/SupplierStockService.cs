using Application.Supplier.Interfaces;
using System;

namespace SupplierWebService
{
    // To be implemented!
    public class SupplierStockService: ISupplierStockService
    {
        public float GetQuantity(string url, string productId)
        {
            Random rand = new Random();
            return rand.Next(0, 100);
        }
        public bool AddProduct(string url, string productId)
        {
            return true;
        }

        public bool WithdrawProduct(string url, string productId, float productQuantity)
        {
            return true;
        }

        public bool UpdateProduct(string url, string productId, float productQuantity)
        {
            return true;
        }
    }
}
