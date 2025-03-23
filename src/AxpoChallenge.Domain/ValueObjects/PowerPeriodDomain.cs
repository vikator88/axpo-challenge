namespace AxpoChallenge.Domain.ValueObjects;

public readonly struct PowerPeriodDomain
{
    public int Id { get; }
    public double Volume { get; }

    public PowerPeriodDomain(int id, double volume)
    {
        Id = id;
        Volume = volume;
    }
}
