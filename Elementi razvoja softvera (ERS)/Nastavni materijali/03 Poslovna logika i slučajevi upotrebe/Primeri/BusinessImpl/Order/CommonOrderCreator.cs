using Domain;
using Domain.Helpers;
using Application.Order.Interfaces;
using IBusinessImpl.Order;
using Application.Common.Interfaces;
using System.Linq;
using Application.Customer.Queries;

namespace BusinessImpl.Order
{
    public class CommonOrderCreator : IOrderCreator
    {
        protected CustomerEntity customer;
        public ICustomerFindByIdQuery CustomerFindByIdQuery { private get; set; }
        protected ProceedingData proceedingData;        
        protected IDiscountCreator discountCreator;

        public ITimeAssigner TimeAssigner { get; set; }

        public CommonOrderCreator(ICustomerFindByIdQuery customerFindByIdQuery, IDiscountCreator discountCreator, ITimeAssigner timeAssigner, ProceedingData proceedingData)
        {
            this.CustomerFindByIdQuery = customerFindByIdQuery;
            this.TimeAssigner = timeAssigner;
            this.proceedingData = proceedingData;
            this.discountCreator = discountCreator;
            ApplyProceedingData();
        }

        private void ApplyProceedingData()
        {
            customer = CustomerFindByIdQuery.FindById(proceedingData.ProceedingCustomerId);
            if (string.IsNullOrEmpty(proceedingData.ProceedingCity))
            {
                proceedingData.ProceedingCity = customer.City;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingHouseNumber))
            {
                proceedingData.ProceedingHouseNumber = customer.HouseNumber;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingPhoneNumber))
            {
                proceedingData.ProceedingPhoneNumber = customer.PhoneNumber;
            }
            if (string.IsNullOrEmpty(proceedingData.ProceedingStreet))
            {
                proceedingData.ProceedingStreet = customer.Street;
            }
        }

        public OrderEntity ApplyShoppingCart()
        {
            OrderEntity order = new OrderEntity
            {
                OrderTime = TimeAssigner.DateTime
            };
            ApplyProductItems(order);
            ApplyOrderHeader(order);
            return order;
        }

        private void ApplyOrderHeader(OrderEntity order)
        {
            order.Customer = customer;
            ApplyDelivery(order);
            order.InitialTotalPrice = this.customer.ShoppingCart.Sum(t => t.UnitPrice * t.Quantity);            
            order.FinalTotalPrice = GetFinalTotalPrice(order);
        }

        private void ApplyProductItems(OrderEntity order)
        {
            order.Items.AddRange(this.customer.ShoppingCart);
        }

        private void ApplyDelivery(OrderEntity order)
        {
            order.ProceedingCity = this.proceedingData.ProceedingCity;
            order.ProceedingHouseNumber = this.proceedingData.ProceedingHouseNumber;
            order.ProceedingPhoneNumber = this.proceedingData.ProceedingPhoneNumber;
            order.ProceedingStreet = this.proceedingData.ProceedingStreet;
        }

        private float GetFinalTotalPrice(OrderEntity order)
        {
            float discountPercentage = this.discountCreator.GetDiscount(order).GetDiscountPercentage();
            if (discountPercentage == 0f)
            {
                return order.InitialTotalPrice;
            }
            return (1f - discountPercentage / 100f) * order.InitialTotalPrice;
        }
    }
}
