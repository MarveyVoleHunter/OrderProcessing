using OrderProcessing.Domain.Entities;
using System;
namespace OrderProcessing.Domain.Services
{
    public static class CustomerSpendCalculator
    {
        public static decimal Calculate(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            // If no orders, spend is zero
            if (customer.Orders == null || customer.Orders.Count == 0)
                return 0m;

            return customer.Orders.Sum(o => o.Total);
        }
    }
}
