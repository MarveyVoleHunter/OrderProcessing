using OrderProcessing.Domain.Entities;

namespace OrderProcessing.DiscountService.DiscountRules
{
    public interface IDiscountRule
    {
        bool IsApplicable(Customer customer, Order order);
        void ApplyDiscountToCustomer(Customer customer);
        void ApplyDiscountToOrder(Customer customer, Order order);
    }
}
