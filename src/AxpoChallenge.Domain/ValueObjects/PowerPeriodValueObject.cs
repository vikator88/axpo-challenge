namespace AxpoChallenge.Domain.ValueObjects;

public readonly struct PowerPeriodValueObject
{
    public int Period { get; }
    public double Volume { get; }

    public PowerPeriodValueObject(int period, double volume)
    {
        Period = period;
        Volume = volume;
    }
}
