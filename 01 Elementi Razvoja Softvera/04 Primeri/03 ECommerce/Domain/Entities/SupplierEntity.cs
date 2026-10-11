using Domain.Comparing;
using System;

namespace Domain
{
    public class SupplierEntity: Entity
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }

        public override object GetId()
        {
            return SupplierId;
        }

        public override bool Equals(object obj)
        {
            if (this == (SupplierEntity)obj)
            {
                return true;
            }
            return ComparerFactory.Instance.GetComparer<SupplierEntity>().Compare(this, (SupplierEntity)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
