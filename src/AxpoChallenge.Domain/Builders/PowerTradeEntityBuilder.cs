using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Domain.Builders;

public class PowerTradeEntityBuilder
{
    private DateTime _tradeDate;
    private string _tradeId;
    private PowerPeriodValueObject[] _periods;

    public PowerTradeEntityBuilder WithTradeDate(DateTime tradeDate)
    {
        _tradeDate = tradeDate;
        return this;
    }

    public PowerTradeEntityBuilder WithTradeId(string tradeId)
    {
        _tradeId = tradeId;
        return this;
    }

    public PowerTradeEntityBuilder WithPeriods(PowerPeriodValueObject[] periods)
    {
        _periods = periods;
        return this;
    }

    public PowerTradeEntity Build()
    {
        return new PowerTradeEntity(_tradeId, _tradeDate, _periods);
    }
}
