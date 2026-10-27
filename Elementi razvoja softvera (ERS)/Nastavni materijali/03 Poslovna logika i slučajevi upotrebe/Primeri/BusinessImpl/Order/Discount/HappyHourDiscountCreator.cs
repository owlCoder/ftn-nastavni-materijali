using Domain;
using Application.Order.Interfaces;

namespace BusinessImpl.Order.Discount
{
    public class HappyHourDiscountCreator : IDiscountCreator
    {
        public IDiscount Discount { private get; set; }
        public IDiscount NoDiscount { private get; set; }

        public HappyHourDiscountCreator(IDiscount discount, IDiscount noDiscount) : base() {
            this.Discount = discount;
            this.NoDiscount = noDiscount;
        }

        public IDiscount GetDiscount(OrderEntity order)
        {
            var orderTime = order.OrderTime;
            if (orderTime.Hour >= 16 && orderTime.Hour <= 17)
            {
                return Discount;
            }
            return NoDiscount;
        }
    }
}
