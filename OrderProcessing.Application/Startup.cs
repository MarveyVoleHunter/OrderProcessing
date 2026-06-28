using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderProcessing.DataStore;
using OrderProcessing.DiscountService;
using OrderProcessing.DiscountService.DiscountRules;
using OrderProcessing.ImportService;
using OrderProcessing.ImportService.CsvReaders;

namespace OrderProcessing.Application
{
    internal class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var discountRules = new List<IDiscountRule>
            {
                new CaliforniaDiscountRule(),
            };
            
            services.AddAutoMapper(cfg => { cfg.AddProfile<ImportMappingProfile>(); });
            services.AddLogging(builder => builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                .AddConsole());
            services.AddSingleton<CustomerCsvReader>();
            services.AddSingleton<OrderCsvReader>();
            services.AddSingleton<OrderItemCsvReader>();
            services.AddSingleton<IEnumerable<IDiscountRule>>(discountRules);
            services.AddSingleton<DataImporter>();
            services.AddSingleton<DiscountCalculator>();
            services.AddSingleton<IRepository, Repository>();
        }
    }
}
