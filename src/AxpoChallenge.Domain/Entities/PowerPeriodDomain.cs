namespace AxpoChallenge.Domain.Entities
{
    public sealed class PowerPeriodDomain
    {
        public PowerPeriodDomain(int period, double volume)
        {
            Period = period;
            Volume = volume;
        }

        public double Period { get; }
        public double Volume { get; }
    }
}
