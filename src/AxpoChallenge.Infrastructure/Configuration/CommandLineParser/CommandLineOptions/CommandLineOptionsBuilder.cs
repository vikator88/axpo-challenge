namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public class CommandLineOptionsBuilder
{
    private int _executionIntervalMinutes;
    private DateTime _executionDate;
    private string _csvOutputFolder;
    private string _timeZone;

    public CommandLineOptionsBuilder WithExecutionIntervalMinutes(int executionIntervalMinutes)
    {
        _executionIntervalMinutes = executionIntervalMinutes;
        return this;
    }

    public CommandLineOptionsBuilder WithExecutionDate(DateTime executionDate)
    {
        _executionDate = executionDate;
        return this;
    }

    public CommandLineOptionsBuilder WithCsvOutputFolder(string csvOutputFolder)
    {
        _csvOutputFolder = csvOutputFolder;
        return this;
    }

    public CommandLineOptionsBuilder WithTimeZone(string timeZone)
    {
        _timeZone = timeZone;
        return this;
    }

    public CommandLineOptions Build()
    {
        return new CommandLineOptions(
            _executionIntervalMinutes,
            _executionDate,
            _csvOutputFolder,
            _timeZone
        );
    }
}
