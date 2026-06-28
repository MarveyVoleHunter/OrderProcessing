using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.DiscountService;
using OrderProcessing.Domain.Entities;
using OrderProcessing.ImportService.CsvReaders;

namespace OrderProcessing.ImportService
{
    internal class Program
    {
        private IEnumerable<Customer> _customers;
        private IEnumerable<Order> _orders;
        private IEnumerable<OrderItem> _orderItems;
        private DiscountCalculator _discountCalculator;

        static void Main(string[] args)
        {
            var program = new Program();
            program.DoImports();
        }

        private void DoImports()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFiles");

            var services = new ServiceCollection();

            Startup.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            var customerReader = serviceProvider.GetRequiredService<CustomerCsvReader>();
            var orderReader = serviceProvider.GetRequiredService<OrderCsvReader>();
            var orderItemReader = serviceProvider.GetRequiredService<OrderItemCsvReader>();

            var customerDtos = customerReader.Read(Path.Combine(folderPath, "Customers.csv"));
            var orderDtos = orderReader.Read(Path.Combine(folderPath, "Orders.csv"));
            var orderItemDtos = orderItemReader.Read(Path.Combine(folderPath, "OrderItems.csv"));
            _discountCalculator = serviceProvider.GetRequiredService<DiscountCalculator>();

            _customers = mapper.Map<IEnumerable<Customer>>(customerDtos);
            _orders = mapper.Map<IEnumerable<Order>>(orderDtos);
            _orderItems = mapper.Map<IEnumerable<OrderItem>>(orderItemDtos);

            AssociateEntities();
            ApplyExistingDiscounts();
            ApplyNewDiscounts();
        }

        private void AssociateEntities()
        {
            foreach (var order in _orders)
            {
                order.AddItems(_orderItems.Where(i => i.OrderId.Equals(order.OrderId)));
            }

            foreach (var customer in _customers)
            {
                customer.Orders.AddRange(_orders.Where(o => o.CustomerId.Equals(customer.CustomerId)));
            }
        }

        private void ApplyExistingDiscounts()
        {
            foreach(var item in _orderItems)
            {
                item.DiscountValue = Math.Round(item.ListPrice * item.DiscountPercentage, 2);
            }           
        }

        private void ApplyNewDiscounts()
        {
            foreach (var customer in _customers)
            {
                _discountCalculator.ApplyDiscountsToCustomer(customer);
            }
        }

        //private void CalculateOrderTotals()
        //{
        //    foreach (var order in _orders)
        //    {
        //        order.Total = OrderTotalCalculator.Calculate(order);
        //    }
        //}
//
        //private void CalculateTotalSpend()
        //{
        //    foreach (var customer in _customers)
        //    {
        //        customer.TotalSpend = CustomerSpendCalculator.Calculate(customer);
        //    }
        //}
    }
}
