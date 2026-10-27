using BusinessImpl.Order;
using BusinessImpl.Order.Discount;
using BusinessImpl.Common;
using Domain;
using Domain.Helpers;
using Domain.Results;
using WebAPI.WebAPIModel;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.Order.Handlers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutOrderController : BaseController
    {
        public ITimeAssigner TimeAssigner { private get; set; }

        public CheckoutOrderController() : base()
        {
            this.TimeAssigner = new NormalTimeAssigner();
        }

        [HttpGet("{customerId}", Name = "GetCheckoutOrder")]
        public OrderResult Get(string customerId)
        {
            return new OrderResult(GetOrder(new ProceedingData() { ProceedingCustomerId = customerId }));
        }

        [HttpGet("{customerId}/{city}/{street}/{houseNumber}/{phoneNumber}", Name = "GetCheckoutOrderWithDeliveryData")]
        public OrderResult Get(string customerId, string city, string street, string houseNumber, string phoneNumber)
        {
            ProceedingData proceedingData = new ProceedingData(city, street, houseNumber, phoneNumber, customerId);
            OrderEntity order = GetOrder(proceedingData);
            order.Apply(proceedingData);
            return new OrderResult(order);
        }

        private OrderEntity GetOrder(ProceedingData proceedingData)
        {
            var orderCreator = new CommonOrderCreator(repoFactory.CustomerFindByIdQuery,
                new HappyHourDiscountCreator(
                new PhoneNumberEndDiscount(repoFactory.CustomerFindByIdQuery, proceedingData.ProceedingCustomerId), 
                new NoDiscount()), TimeAssigner, proceedingData);
            var checkoutHandler = new CheckoutHandler(repoFactory, orderCreator, this.TimeAssigner, proceedingData) { TimeAssigner = TimeAssigner };
            Result result = checkoutHandler.Checkout();
            if ((result == null || result.ResultObject == null || !(result.ResultObject is OrderEntity)))
            {
                return new OrderEntity();
            }
            return (OrderEntity)result.ResultObject;
        }
    }
}
