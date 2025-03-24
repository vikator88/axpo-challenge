namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public interface ICommandLineOptionValidator
{
    int ValidateInterval(Dictionary<string, string> options);
    DateTime ValidateExecutionDate(Dictionary<string, string> options);
    string ValidateOutputFolder(Dictionary<string, string> options);
    string ValidateTimeZone(Dictionary<string, string> options);
}
