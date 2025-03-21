using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Application;

public interface IPowerTradeRepository
{
    Task<IEnumerable<PowerTradeDomain>> GetTradesByDateAsync(DateTime date);
}
