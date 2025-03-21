using Axpo;
using AxpoChallenge.Domain.Entities;

namespace AxpoChallenge.Infrastructure.Mapping;

public static class PowerPeriodMapper
{
    public static PowerPeriodDomain MapToDomain(PowerPeriod externalPeriod)
    {
        return new PowerPeriodDomain(externalPeriod.Period, externalPeriod.Volume);
    }

    public static IEnumerable<PowerPeriodDomain> MapToDomain(
        IEnumerable<PowerPeriod> externalPeriods
    )
    {
        return externalPeriods.Select(p => MapToDomain(p));
    }
}
