using Axpo;
using AxpoChallenge.Application;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Infrastructure.Mapping;

namespace AxpoChallenge.Infrastructure;

public class PowerTradeRepository : IPowerTradeRepository
{
    private readonly IPowerService _powerService;

    public PowerTradeRepository(IPowerService powerService)
    {
        _powerService = powerService;
    }

    public async Task<IEnumerable<PowerTradeDomain>> GetTradesByDateAsync(DateTime date)
    {
        // Extracting the trades from the external service
        IEnumerable<PowerTrade> trades = await _powerService.GetTradesAsync(date);

        // Mapping the trades to the domain model
        return PowerTradeMapper.MapToDomain(trades);
    }
}
