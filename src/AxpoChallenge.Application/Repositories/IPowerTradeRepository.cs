using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Application.Repositories;

public interface IPowerTradeRepository
{
    /// <summary>
    /// Get power trades for a given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<IEnumerable<PowerTradeEntity>> GetTradesByDateAsync(DateTime date);
}
