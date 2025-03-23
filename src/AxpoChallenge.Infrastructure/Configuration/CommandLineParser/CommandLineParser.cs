using System.Globalization;
using System.Runtime.InteropServices;

namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public class CommandLineParser : ICommandLineParser
{
    public CommandLineOptionsDTO Parse(string[] args)
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

        if (options.ContainsKey("-h") || options.ContainsKey("--help"))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        // Validate and parse the interval
        int interval = 0;
        if (
            !options.ContainsKey("-t")
            || !int.TryParse(options["-t"], out interval)
            || interval <= 0
        )
        {
            Console.WriteLine(
                "Error: Invalid interval value. Please provide a valid interval in minutes. Use -h or --help for more information."
            );
            Environment.Exit(1);
        }

        //Validate and parse the execution date
        DateTime executionDate = DateTime.Now.Date; // Default: local Date
        if (
            options.ContainsKey("-d")
            && !DateTime.TryParseExact(
                options["-d"],
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out executionDate
            )
        )
        {
            Console.WriteLine(
                "Error: Invalid date value. Please provide a valid date in the format YYYYMMDD. Use -h or --help for more information."
            );
            Environment.Exit(1);
        }

        // Validate and parse the output folder

        string outputFolder = options.ContainsKey("-o")
            ? options["-o"]
            : Directory.GetCurrentDirectory();

        // check tah folder exists
        if (!Directory.Exists(outputFolder))
        {
            Console.WriteLine(
                "Error: Invalid output folder. Please provide a valid folder path. Use -h or --help for more information."
            );
            Environment.Exit(1);
        }

        // Validate and parse the time zone
        if (
            options.ContainsKey("-tz")
            && !TimeZoneInfo.GetSystemTimeZones().Any(tz => tz.Id == options["-tz"])
        )
        {
            Console.WriteLine(
                "Error: Invalid time zone value. Please provide a valid time zone. Use -h or --help for more information."
            );
            Environment.Exit(1);
        }

        string timeZone = options.ContainsKey("-tz")
            ? options["-tz"]
            : (
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Central European Standard Time"
                    : "Europe/Berlin"
            );

        return new CommandLineOptionsDTOBuilder()
            .WithExecutionIntervalMinutes(interval)
            .WithExecutionDate(executionDate)
            .WithCsvOutputFolder(outputFolder)
            .WithTimeZone(timeZone)
            .Build();
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
            "                                        Generates the report for day-ahead. Default: current date."
        );
        Console.WriteLine(
            "  -o <outputFolder>          (Optional) Folder where the CSV files will be saved. Default: current directory."
        );
        Console.WriteLine(
            "  -tz <timeZoneString>       (Optional) Time zone of the server. Keep in mind your OS."
        );
        Console.WriteLine(
            "                                        Default: Central European Standard Time (Windows) or Europe/Berlin (Unix-like)."
        );
        Console.WriteLine("  -h, --help                 Show this help.");
    }
}
