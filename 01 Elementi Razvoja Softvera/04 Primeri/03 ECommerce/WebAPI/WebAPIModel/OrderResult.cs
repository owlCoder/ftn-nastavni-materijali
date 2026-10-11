using Domain;

namespace WebAPI.WebAPIModel
{
    public class OrderResult: BaseWebAPIResult
    {
        public string OrderId { get; set; }
        public float TotalAmount { get; set; }
        public float AppliedDiscount { get; set; }


        public OrderResult(OrderEntity order): base()
        {
            this.OrderId = order.OrderId;
            this.TotalAmount = order.FinalTotalPrice;
            this.AppliedDiscount = order.AppliedDiscount;
        }

        public OrderResult(string message) : base(message) { }

        protected override void InitProps()
        {
            this.OrderId = string.Empty;
            this.TotalAmount = 0;
            this.AppliedDiscount = 0;
        }
    }
}
