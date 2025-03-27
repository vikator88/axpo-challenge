using Axpo;
using AxpoChallenge.Domain.Builders;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Infrastructure.Mapping;

public static class PowerTradeMapper
{
    public static PowerTradeEntity MapToDomain(PowerTrade externalTrade)
    {
        PowerPeriodValueObject[] powerPeriods = PowerPeriodMapper
            .MapToDomain(externalTrade.Periods)
            .ToArray();

        // Mapping the power trade entity using the builder
        PowerTradeEntity mappedPowerTrade = new PowerTradeEntityBuilder()
            .WithTradeId(externalTrade.TradeId)
            .WithTradeDate(externalTrade.Date)
            .WithPeriods(powerPeriods)
            .Build();

        return mappedPowerTrade;
    }

    public static IEnumerable<PowerTradeEntity> MapToDomain(IEnumerable<PowerTrade> externalTrades)
    {
        return externalTrades.Select(t => MapToDomain(t));
    }
}
