using Axpo;
using AxpoChallenge.Application.Repositories;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Infrastructure.Mapping;
using Microsoft.Extensions.Logging;
using Polly;

namespace AxpoChallenge.Infrastructure.Repositories;

public class PowerTradeRepository : IPowerTradeRepository
{
    private readonly IPowerService _powerService;
    private readonly ILogger<PowerTradeRepository> _logger;

    public PowerTradeRepository(IPowerService powerService, ILogger<PowerTradeRepository> logger)
    {
        _powerService = powerService;
        _logger = logger;
    }

    public async Task<IEnumerable<PowerTradeEntity>> GetTradesByDateAsync(DateTime date)
    {
        _logger.LogInformation($"Getting trades for date: {date:dd/MM/yyyy}");
        // Retry policy for handling PowerServiceException error implemented with Polly
        // Max 4 retries with exponential backoff: 2, 4, 8, 16 seconds
        var retryPolicy = Policy
            .Handle<PowerServiceException>() // Handles the exception thrown by the external service
            .WaitAndRetryAsync(
                retryCount: 4,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential: 2, 4, 8 seconds
                onRetry: (exception, timeSpan, retryCount, context) => // Log the retry attempts
                {
                    _logger.LogWarning(
                        exception,
                        $"Retry {retryCount} after {timeSpan.Seconds} seconds."
                    );
                }
            );

        // Extracting the trades from the external service with retry policy
        IEnumerable<PowerTrade> trades = await retryPolicy.ExecuteAsync(
            () => _powerService.GetTradesAsync(date)
        );

        // Mapping the trades to the domain model
        return PowerTradeMapper.MapToDomain(trades);
    }
}
