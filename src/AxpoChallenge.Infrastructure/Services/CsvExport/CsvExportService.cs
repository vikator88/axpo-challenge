using System.Globalization;
using AxpoChallenge.Application.Services;
using AxpoChallenge.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Polly;

namespace AxpoChallenge.Infrastructure.Services.CsvExport;

public class CsvExportService : ICsvExportService
{
    private readonly ILogger<CsvExportService> _logger;

    public CsvExportService(ILogger<CsvExportService> logger)
    {
        _logger = logger;
    }

    public async Task ExportAsync<T>(IEnumerable<T> data, string fullDestinationPath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) // InvariantCulture uses the dot as decimal separator
        {
            Delimiter = ";", // Column separator
            HasHeaderRecord = true, // Include headers
            NewLine = Environment.NewLine, // New line character
        };

        using var writer = new StreamWriter(fullDestinationPath);
        using var csv = new CsvWriter(writer, config);

        // Create a class map for the given type if it exists
        // If not, the default mapping will be used
        ClassMap map = CsvClassMapFactory.Create<T>();
        if (map != null)
        {
            csv.Context.RegisterClassMap(map);
        }

        await csv.WriteRecordsAsync(data);
        _logger.LogInformation($"Data exported to {fullDestinationPath}");
    }
}
