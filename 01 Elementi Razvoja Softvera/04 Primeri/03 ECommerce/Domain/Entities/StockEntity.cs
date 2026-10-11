using System.Collections.Generic;

namespace Domain
{
    public abstract class StockEntity : Entity
    {
        public StockEntity()
        {
            ProductStocks = new List<ProductStock>();
        }
        
        public int StockId { get; set; }
        public string StockName { get; set; }

        public bool IsDefault { get; set; }
        public List<ProductStock> ProductStocks { get; set; }

        public override object GetId()
        {
            return StockId;
        }        
    }
}
