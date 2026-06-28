using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.DataStore;
using OrderProcessing.DiscountService;
using OrderProcessing.Domain.Entities;
using OrderProcessing.ImportService;

namespace OrderProcessing.Application
{
    internal class Program
    {
        //private DiscountCalculator _discountCalculator;

        static void Main(string[] args)
        {
            var program = new Program();
            program.DoImports();
        }

        private void DoImports()
        {
            var services = new ServiceCollection();

            Startup.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var importer = serviceProvider.GetRequiredService<DataImporter>();
            var repository = serviceProvider.GetRequiredService<IRepository>();
        

            importer.DoImports();
            var customers = repository.GetAllCustomers();

            //ApplyNewDiscounts(customers);
        }

        //private void ApplyNewDiscounts(IEnumerable<Customer> customers)
        //{
        //    var disc = 
        //    foreach (var customer in customers)
        //    {
        //        _discountCalculator.ApplyDiscountsToCustomer(customer);
        //    }
        //}

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
