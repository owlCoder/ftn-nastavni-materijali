using Domain.Comparing;
using System;

namespace Domain
{
    public class SupplierStockEntity: StockEntity
    {
        public SupplierEntity Supplier { get; set; }
        public string WebServiceURL { get; set; }


        public override bool Equals(object obj)
        {
            ComparerFactory.Instance.GetComparer<SupplierStockEntity>().IgnoreMember("ProductStocks");
            return ComparerFactory.Instance.GetComparer<SupplierStockEntity>().Compare(this, (SupplierStockEntity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
