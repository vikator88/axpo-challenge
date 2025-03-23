using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Domain.Entities
{
    public sealed class PowerTradeEntity
    {
        public PowerTradeEntity(string tradeId, DateTime date, PowerPeriodValueObject[] periods)
        {
            TradeId = tradeId;
            Date = date;
            Periods = periods;
        }

        public string TradeId { get; }
        public DateTime Date { get; }
        public PowerPeriodValueObject[] Periods { get; }
    }
}
