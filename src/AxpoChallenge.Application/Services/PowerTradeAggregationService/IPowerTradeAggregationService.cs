using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Application.Services;

public interface IPowerTradeAggregationService
{
    IEnumerable<AggregatedPowerPosition> AggregateTrades(IEnumerable<PowerTradeDomain> trades);
}
