using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class ShoppingCartEntity : List<ProductItem>
    {
        public ShoppingCartEntity() { }

        public new void Add(ProductItem item)
        {
            ProductItem updatingItem = this.Where(t => t.Product.ProductId == item.Product.ProductId).FirstOrDefault();
            if (!(updatingItem == null))
            {
                updatingItem.Quantity += item.Quantity;
                return;
            }
            base.Add(item);
        }



    }
}
