namespace AxpoChallenge.Domain.Entities;

public sealed class AggregatedPowerPosition
{
    public AggregatedPowerPosition(DateTime dateTimeUtc)
    {
        DateTimeUtc = dateTimeUtc;
        Volume = 0;
    }

    public DateTime DateTimeUtc { get; }
    public double Volume { get; private set; }

    public void AddVolume(double volume)
    {
        Volume += volume;
    }
}
