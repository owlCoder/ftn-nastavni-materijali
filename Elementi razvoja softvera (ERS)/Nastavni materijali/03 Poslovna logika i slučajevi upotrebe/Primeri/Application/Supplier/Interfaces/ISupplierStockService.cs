namespace Application.Supplier.Interfaces
{
    public interface ISupplierStockService
    {
        float GetQuantity(string url, string productId);
        bool AddProduct(string url, string productId);

        bool WithdrawProduct(string url, string productId, float productQuantity);

        public bool UpdateProduct(string url, string productId, float productQuantity);
    }
}
