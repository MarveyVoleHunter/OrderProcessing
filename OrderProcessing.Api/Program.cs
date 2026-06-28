using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessing.DataStore;
using OrderProcessing.DiscountService;
using OrderProcessing.DiscountService.DiscountRules;
using OrderProcessing.ImportService;
using OrderProcessing.ImportService.CsvReaders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRepository, Repository>();

ConfigureServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var importer = scope.ServiceProvider.GetRequiredService<DataImporter>();
    importer.DoImports();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services)
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
