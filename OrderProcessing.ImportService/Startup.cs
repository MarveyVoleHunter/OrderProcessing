using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderProcessing.DiscountService;
using OrderProcessing.DiscountService.DiscountRules;
using OrderProcessing.ImportService.CsvReaders;

namespace OrderProcessing.ImportService
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
            services.AddSingleton<DiscountService.DiscountCalculator>();
            //services.AddSingleton(sp => {
            //var logger = services.BuildServiceProvider.get
            //return new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    PrepareHeaderForMatch = args => args.Header.Replace("_", "").ToLower(),

            //    ReadingExceptionOccurred = (ex) =>
            //    {
            //        _logger.LogWarning("Invalid data encountered.  Details: {0}", ex.Exception.Message);
            //        return false;
            //    }
            //});
        }
    }
}
