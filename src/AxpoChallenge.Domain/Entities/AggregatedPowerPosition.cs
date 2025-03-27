namespace AxpoChallenge.Domain.Entities;

public sealed class AggregatedPowerPosition
{
    public AggregatedPowerPosition(DateTime dateTimeUtc)
    {
        Id = Guid.NewGuid();
        DateTimeUtc = dateTimeUtc;
        Volume = 0;
    }

    public Guid Id { get; }
    public DateTime DateTimeUtc { get; }
    public double Volume { get; private set; }

    public void AddVolume(double volume)
    {
        Volume += volume;
    }
}
