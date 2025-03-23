namespace AxpoChallenge.Domain.ValueObjects;

public readonly struct PowerPeriodValueObject
{
    public int Id { get; }
    public double Volume { get; }

    public PowerPeriodValueObject(int id, double volume)
    {
        Id = id;
        Volume = volume;
    }
}
