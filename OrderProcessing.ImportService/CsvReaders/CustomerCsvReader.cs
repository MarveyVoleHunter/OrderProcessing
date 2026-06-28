using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using OrderProcessing.Domain.DTOs;
using System.Globalization;

namespace OrderProcessing.ImportService.CsvReaders
{
    internal class CustomerCsvReader
    {
        private readonly ILogger _logger;

        public CustomerCsvReader(ILogger<CustomerCsvReader> logger)
        {
            _logger = logger;
        }

        public IEnumerable<CustomerDto> Read(string filePath)
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

            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, config);

            return [..csvReader.GetRecords<CustomerDto>()];
        }
    }
}
