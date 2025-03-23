using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Application.Repositories;
using AxpoChallenge.Application.Services;
using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Application.UseCases.ExportPowerTrades;

public class ExportPowerTradesUseCase : IExportPowerTradesUseCase
{
    private readonly IPowerTradeRepository _powerTradeRepository;
    private readonly ICsvExportService _csvExportService;
    private readonly IPowerTradeAggregationService _powerTradeAggregationService;

    public ExportPowerTradesUseCase(
        IPowerTradeRepository powerTradeRepository,
        ICsvExportService csvExportService,
        IPowerTradeAggregationService powerTradeAggregationService
    )
    {
        _powerTradeRepository = powerTradeRepository;
        _csvExportService = csvExportService;
        _powerTradeAggregationService = powerTradeAggregationService;
    }

    public async Task ExecuteAsync(DateTime date, string destinationFolder)
    {
        // Get the datetime in UTC of the execution time to be used in the filename
        DateTime executionTimeUtc = DateTime.UtcNow;

        // Get power trades for the given date
        IEnumerable<PowerTradeDomain> powerTrades =
            await _powerTradeRepository.GetTradesByDateAsync(date);

        // Aggregate power trades
        IEnumerable<AggregatedPowerPosition> aggregatedPowerTrades =
            _powerTradeAggregationService.AggregateTrades(powerTrades);

        // Create the destination path for the CSV file
        string destinationPath =
            $"{destinationFolder}/PowerTrades_{date:yyyyMMdd}_{executionTimeUtc:yyyyMMddHHmm}.csv";

        // Export power trades to a CSV file
        await _csvExportService.ExportAsync(aggregatedPowerTrades, destinationPath);
    }
}
