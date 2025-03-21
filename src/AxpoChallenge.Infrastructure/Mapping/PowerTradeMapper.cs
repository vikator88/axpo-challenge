using Axpo;
using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Infrastructure.Mapping;

public static class PowerTradeMapper
{
    public static PowerTradeDomain MapToDomain(PowerTrade externalTrade)
    {
        PowerTradeDomain mappedPowerTrade = new PowerTradeDomain(
            externalTrade.TradeId,
            externalTrade.Date,
            PowerPeriodMapper.MapToDomain(externalTrade.Periods).ToArray() // Mapping the periods
        );

        return mappedPowerTrade;
    }

    public static IEnumerable<PowerTradeDomain> MapToDomain(IEnumerable<PowerTrade> externalTrades)
    {
        return externalTrades.Select(t => MapToDomain(t));
    }
}
