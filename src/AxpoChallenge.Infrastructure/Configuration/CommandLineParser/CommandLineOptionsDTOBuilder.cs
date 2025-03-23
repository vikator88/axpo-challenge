namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public class CommandLineOptionsDTOBuilder
{
    private int _executionIntervalMinutes;
    private DateTime _executionDate;
    private string _csvOutputFolder;
    private string _timeZone;

    public CommandLineOptionsDTOBuilder WithExecutionIntervalMinutes(int executionIntervalMinutes)
    {
        _executionIntervalMinutes = executionIntervalMinutes;
        return this;
    }

    public CommandLineOptionsDTOBuilder WithExecutionDate(DateTime executionDate)
    {
        _executionDate = executionDate;
        return this;
    }

    public CommandLineOptionsDTOBuilder WithCsvOutputFolder(string csvOutputFolder)
    {
        _csvOutputFolder = csvOutputFolder;
        return this;
    }

    public CommandLineOptionsDTOBuilder WithTimeZone(string timeZone)
    {
        _timeZone = timeZone;
        return this;
    }

    public CommandLineOptionsDTO Build()
    {
        return new CommandLineOptionsDTO(
            _executionIntervalMinutes,
            _executionDate,
            _csvOutputFolder,
            _timeZone
        );
    }
}
