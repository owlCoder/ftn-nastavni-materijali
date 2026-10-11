using Domain.Comparing;
using System;

namespace Domain
{
    public class LocalStockEntity: StockEntity
    {
        public float Capacity { get; set; }
        public string Address { get; set; }
        public override bool Equals(object obj)
        {
            
            return ComparerFactory.Instance.GetComparer<LocalStockEntity>().Compare(this, (LocalStockEntity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
