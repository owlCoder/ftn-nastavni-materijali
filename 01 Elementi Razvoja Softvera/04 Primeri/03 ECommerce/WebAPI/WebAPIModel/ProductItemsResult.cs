using Domain;
using System.Collections.Generic;

namespace WebAPI.WebAPIModel
{
    public class ProductItemsResult : BaseWebAPIResult
    {
        public List<ProductItemResult> ProductItems { get; set; }

        public ProductItemsResult(List<ProductItem> productItems)
        {
            ApplyProductItems(productItems);
        }

        public ProductItemsResult(string message): base(message) { }

        protected override void InitProps()
        {
            ProductItems = new List<ProductItemResult>();
        }

        public void ApplyProductItems(List<ProductItem> productItems)
        {
            productItems.ForEach(t => ProductItems.Add(new ProductItemResult(t)));
        }
    }
}
