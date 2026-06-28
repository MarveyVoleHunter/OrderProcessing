using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.ImportService.CsvReaders;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Services;

namespace OrderProcessing.ImportService
{
    internal class Program
    {
        private IEnumerable<Customer> _customers;
        private IEnumerable<Order> _orders;
        private IEnumerable<OrderItem> _orderItems;

        static void Main(string[] args)
        {
            var program = new Program();
            program.DoImports();
        }

        private void DoImports()
        {
            var folderPath = "C:\\Development\\Technical Assessment\\ImportFiles";

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

            _customers = mapper.Map<IEnumerable<Customer>>(customerDtos);
            _orders = mapper.Map<IEnumerable<Order>>(orderDtos);
            _orderItems = mapper.Map<IEnumerable<OrderItem>>(orderItemDtos);

            AssociateEntities();
            CalculateOrderTotals();
            CalculateTotalSpend();
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

        private void CalculateOrderTotals()
        {
            foreach (var order in _orders)
            {
                order.Total = OrderTotalCalculator.Calculate(order);
            }
        }

        private void CalculateTotalSpend()
        {
            foreach (var customer in _customers)
            {
                customer.TotalSpend = CustomerSpendCalculator.Calculate(customer);
            }
        }
    }
}
