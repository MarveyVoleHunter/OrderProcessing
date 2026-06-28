using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Enums;

namespace OrderProcessing.DiscountService.DiscountRules
{
    public class CaliforniaDiscountRule : IDiscountRule
    {
        public bool IsApplicable(Customer customer, Order? order = null)
        {
            ArgumentNullException.ThrowIfNull(customer);

            return customer.State.Equals("CA", StringComparison.InvariantCultureIgnoreCase);
        }

        public void ApplyDiscountToCustomer(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            foreach (var order in customer.Orders)
            {
                ApplyDiscountToOrder(customer, order);
            }
         }

        public void ApplyDiscountToOrder(Customer customer, Order order)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentNullException.ThrowIfNull(order);

            if (!IsApplicable(customer, order))
                return;

            if (order.HasDiscountBeenApplied(DiscountRule.CaliforniaDiscountRule))
            {
                // This type of discount has already been applied to this order.
                return;
            }

            order.AddDiscountRule(DiscountRule.CaliforniaDiscountRule);

            foreach (var item in order.OrderItems)
            {
                item.DiscountPercentage = 0.05m;
                item.DiscountValue = Math.Round(item.ListPrice * 0.05m, 2);
            }
        }
    }
}
