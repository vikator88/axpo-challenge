namespace AxpoChallenge.Domain.Entities;

public sealed class AggregatedPowerPosition
{
    public AggregatedPowerPosition(DateTime dateTimeUtc, double totalVolume)
    {
        DateTimeUtc = dateTimeUtc;
        TotalVolume = totalVolume;
    }

    public DateTime DateTimeUtc { get; }
    public double TotalVolume { get; }
}
