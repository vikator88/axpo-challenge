using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Domain.Entities
{
    public sealed class PowerTradeDomain
    {
        public PowerTradeDomain(string tradeId, DateTime date, PowerPeriodDomain[] periods)
        {
            TradeId = tradeId;
            Date = date;
            Periods = periods;
        }

        public string TradeId { get; }
        public DateTime Date { get; }
        public PowerPeriodDomain[] Periods { get; }
    }
}
