using Domain.Comparing;
using Domain.Helpers;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class OrderEntity : Entity
    {
       public OrderEntity()
        {
            this.OrderTime = DateTime.Now;
            this.Items = new List<ProductItem>();
            this.OrderId = Guid.NewGuid().ToString();
        }
        public CustomerEntity Customer { get; set; }

        public string OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public float InitialTotalPrice { get; set; }
        public float FinalTotalPrice { get; set; }
        public float AppliedDiscount { get; set; }
        public string ProceedingCity { get; set; }
        public string ProceedingStreet { get; set; }
        public string ProceedingHouseNumber { get; set; }
        public string ProceedingPhoneNumber { get; set; }

        public List<ProductItem> Items { get; set; }


        public override object GetId()
        {
            return OrderId;
        }

        public void Apply(ProceedingData proceedingData)
        {
            this.ProceedingCity = proceedingData.ProceedingCity;
            this.ProceedingHouseNumber = proceedingData.ProceedingHouseNumber;
            this.ProceedingPhoneNumber = proceedingData.ProceedingPhoneNumber;
            this.ProceedingStreet = proceedingData.ProceedingStreet;
        }

        public override bool Equals(object obj)
        {
            if (this == (OrderEntity)obj)
            {
                return true;
            }
            return ComparerFactory.Instance.GetComparer<OrderEntity>().Compare(this, (OrderEntity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
