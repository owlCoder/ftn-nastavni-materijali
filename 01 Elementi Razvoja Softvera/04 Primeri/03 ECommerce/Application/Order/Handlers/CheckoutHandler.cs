using Domain;
using Domain.Helpers;
using Domain.Results;
using IBusinessImpl.Order;
using Application.Common.Interfaces;
using Application.Customer.Commands;
using Application.Order.Commands;
using Application.Common.Factory;

namespace Application.Order.Handlers
{
    public class CheckoutHandler
    {
        public ICustomerInsertOrUpdateCommand CustomerInsertOrUpdateCommand { private get; set; }

        public ICustomerUpdateCommand CustomerUpdateCommand { private get; set; }
        public IOrderInsertCommand OrderInsertCommand { private get; set; }
        public IOrderCreator OrderCreator { private get; set; }
        public ITimeAssigner TimeAssigner { private get; set; }
        public ProceedingData ProceedingData { private get; set; }
        private readonly CustomerEntity customer;

        public CheckoutHandler(IRepoFactory repoFactory, IOrderCreator orderCreator, ITimeAssigner timeAssigner, ProceedingData proceedingData)
        {
            CustomerInsertOrUpdateCommand = repoFactory.CustomerInsertOrUpdateCommand;
            CustomerUpdateCommand = repoFactory.CustomerUpdateCommand;
            OrderInsertCommand = repoFactory.OrderInsertCommand;
            this.TimeAssigner = timeAssigner;
            this.OrderCreator = orderCreator;
            this.ProceedingData = proceedingData;
            this.customer = new CustomerEntity(ProceedingData);
            CustomerInsertOrUpdateCommand.InsertOrUpdate(this.customer);
            this.TimeAssigner = timeAssigner;            
        }

        public CheckoutHandler(IOrderCreator orderCreator)
        {
            this.OrderCreator = orderCreator;
        }


        public Result Checkout()
        {
            Result result = new Result();
            OrderCreator.TimeAssigner = TimeAssigner;
            if (OrderCreator == null)
            {
                result.Log(LogLevel.Error, $"Unknown customer or invalid input parameters");
                return result;
            }
            OrderEntity order = OrderCreator.ApplyShoppingCart();
            if (!OrderInsertCommand.Insert(order))
            {
                result.Log(LogLevel.Error, $"Unsuccesful storing to db");
            } else
            {
                customer.ShoppingCart.Clear();
                CustomerUpdateCommand.Update(customer);
            }

            result.ResultObject = order;
            return result;
        }
    }
}
