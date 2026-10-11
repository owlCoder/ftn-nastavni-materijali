using Domain.Comparing;
using Domain.Helpers;
using System;

namespace Domain
{
    public class CustomerEntity: Entity
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public ShoppingCartEntity ShoppingCart { get; set; }

        public CustomerEntity(CustomerEntity customer): this() {
            this.CustomerId = customer.CustomerId;
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.City = customer.City;
            this.Street = customer.Street;
            this.HouseNumber = customer.HouseNumber;
            this.PhoneNumber = customer.PhoneNumber;
        }

        public CustomerEntity(ProceedingData customer) : this()
        {
            this.City = customer.ProceedingCity;
            this.Street = customer.ProceedingStreet;
            this.HouseNumber = customer.ProceedingHouseNumber;
            this.PhoneNumber = customer.ProceedingPhoneNumber;
            this.CustomerId = customer.ProceedingCustomerId;
        }



        public CustomerEntity()
        {
            this.ShoppingCart = new ShoppingCartEntity();
        }

        public override object GetId()
        {
            return CustomerId;
        }

        public override bool Equals(object obj)
        {
            if (this == (CustomerEntity)obj)
            {
                return true;
            }
            return ComparerFactory.Instance.GetComparer<CustomerEntity>().Compare(this, (CustomerEntity)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }
    }
}
