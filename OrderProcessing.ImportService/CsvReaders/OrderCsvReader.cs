using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Logging;
using OrderProcessing.Domain.DTOs;
using System.Globalization;

namespace OrderProcessing.ImportService.CsvReaders
{
    public class OrderCsvReader
    {
        private readonly ILogger _logger;

        public OrderCsvReader(ILogger<OrderCsvReader> logger)
        {
            _logger = logger;
        }

        public IEnumerable<OrderDto> Read(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("_", "").ToLower(),
                ReadingExceptionOccurred = (ex) =>
                {
                    _logger.LogWarning("Invalid data encountered.  Details: {0}", ex.Exception.Message);
                    return false;
                }
            };
            var options = new TypeConverterOptions { Formats = ["dd/MM/yyyy"] };

            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, config);
            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateOnly>(options);
            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateOnly?>(options);

            return [.. csvReader.GetRecords<OrderDto>()];
        }
    }
}
