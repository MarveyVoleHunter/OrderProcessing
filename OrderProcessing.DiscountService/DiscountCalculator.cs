using OrderProcessing.DiscountService.DiscountRules;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.DiscountService
{
    public class DiscountCalculator
    {
        private readonly IEnumerable<IDiscountRule> _discountRules;

        public DiscountCalculator(IEnumerable<IDiscountRule> discountRules)
        {
            _discountRules = discountRules ?? throw new ArgumentNullException(nameof(discountRules));
        }

        public void ApplyDiscountsToCustomer(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            foreach (var order in customer.Orders)
            {
                ApplyDiscountsToOrder(customer, order);
            }
        }

        public void ApplyDiscountsToOrder(Customer customer, Order order)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentNullException.ThrowIfNull(order);

            foreach (var rule in _discountRules)
            {
                if (rule.IsApplicable(customer, order))
                {
                    rule.ApplyDiscountToOrder(customer, order);
                    break; // Apply only the first applicable discount rule
                }
            }
        }
    }
}
