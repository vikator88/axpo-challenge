using Axpo;
using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Infrastructure.Mapping;

public static class PowerPeriodMapper
{
    public static PowerPeriodValueObject MapToDomain(PowerPeriod externalPeriod)
    {
        return new PowerPeriodValueObject(externalPeriod.Period, externalPeriod.Volume);
    }

    public static IEnumerable<PowerPeriodValueObject> MapToDomain(
        IEnumerable<PowerPeriod> externalPeriods
    )
    {
        return externalPeriods.Select(p => MapToDomain(p));
    }
}
