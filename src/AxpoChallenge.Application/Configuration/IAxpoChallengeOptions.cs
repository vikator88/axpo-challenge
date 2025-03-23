namespace AxpoChallenge.Application.Configuration;

public interface IAxpoChallengeOptions
{
    public int ExecutionIntervalMinutes { get; }
    public DateTime ExecutionDate { get; }
    public string CsvOutputFolder { get; }
    public string TimeZone { get; }
}
