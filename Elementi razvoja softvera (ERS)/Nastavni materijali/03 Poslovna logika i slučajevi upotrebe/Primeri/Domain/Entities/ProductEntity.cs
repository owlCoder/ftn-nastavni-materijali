using Domain.Comparing;
using System;

namespace Domain
{
    public class ProductEntity : Entity
    {
        public ProductEntity()
        {
        }

        public ProductEntity(string productName)
        {
            this.ProductName = productName;
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }

        public SupplierEntity Supplier { get; set; }

        public override object GetId()
        {
            return ProductId;
        }

        public override bool Equals(object obj)
        {
            if (this == (ProductEntity)obj)
            {
                return true;
            }
            return ComparerFactory.Instance.GetComparer<ProductEntity>().Compare(this, (ProductEntity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
