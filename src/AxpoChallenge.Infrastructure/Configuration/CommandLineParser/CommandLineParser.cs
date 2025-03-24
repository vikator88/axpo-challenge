namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public class CommandLineParser : ICommandLineParser
{
    private readonly ICommandLineOptionValidator _validator;

    public CommandLineParser(ICommandLineOptionValidator validator)
    {
        _validator = validator;
    }

    public CommandLineOptions Parse(string[] args)
    {
        var options = ParseArguments(args);

        if (options.ContainsKey("-h") || options.ContainsKey("--help"))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        int interval = _validator.ValidateInterval(options);
        DateTime executionDate = _validator.ValidateExecutionDate(options);
        string outputFolder = _validator.ValidateOutputFolder(options);
        string timeZone = _validator.ValidateTimeZone(options);

        return new CommandLineOptionsBuilder()
            .WithExecutionIntervalMinutes(interval)
            .WithExecutionDate(executionDate)
            .WithCsvOutputFolder(outputFolder)
            .WithTimeZone(timeZone)
            .Build();
    }

    private Dictionary<string, string> ParseArguments(string[] args)
    {
        var options = new Dictionary<string, string>();
        string currentOptionKey = null;

        foreach (string arg in args)
        {
            if (arg.StartsWith("-"))
            {
                currentOptionKey = arg;
                options[currentOptionKey] = null;
            }
            else if (currentOptionKey != null)
            {
                options[currentOptionKey] = arg;
                currentOptionKey = null;
            }
        }
        return options;
    }

    private void ShowHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine(
            "  AxpoChallenge.exe -t <intervalInMinutes> [-d <executionDate>] [-o <outputFolder>] [-tz <timeZoneString>]"
        );
        Console.WriteLine("\nOptions:");
        Console.WriteLine(
            "  -t <intervalInMinutes>     (Required) Interval in minutes to execute the report generation."
        );
        Console.WriteLine(
            "  -d <executionDate>         (Optional) Date of report generation. Format: YYYYMMDD."
        );
        Console.WriteLine(
            "  -o <outputFolder>          (Optional) Folder where the CSV files will be saved. Default: current directory."
        );
        Console.WriteLine(
            "  -tz <timeZoneString>       (Optional) Time zone of the server. Keep in mind your OS."
        );
        Console.WriteLine("  -h, --help                 Show this help.");
    }
}
