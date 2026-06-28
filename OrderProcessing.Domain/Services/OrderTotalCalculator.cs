using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Domain.Services
{
    public static class OrderTotalCalculator
    {
        public static decimal Calculate(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            // If no items, total is zero
            if (order.OrderItems == null || order.OrderItems.Count == 0)
                return 0m;

            return order.OrderItems.Sum(i => i.ListPrice - i.Discount);
        }
    }
}
