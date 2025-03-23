using Axpo;
using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Infrastructure.Mapping;

public static class PowerTradeMapper
{
    public static PowerTradeEntity MapToDomain(PowerTrade externalTrade)
    {
        PowerTradeEntity mappedPowerTrade = new PowerTradeEntity(
            externalTrade.TradeId,
            externalTrade.Date,
            PowerPeriodMapper.MapToDomain(externalTrade.Periods).ToArray() // Mapping the periods
        );

        return mappedPowerTrade;
    }

    public static IEnumerable<PowerTradeEntity> MapToDomain(IEnumerable<PowerTrade> externalTrades)
    {
        return externalTrades.Select(t => MapToDomain(t));
    }
}
