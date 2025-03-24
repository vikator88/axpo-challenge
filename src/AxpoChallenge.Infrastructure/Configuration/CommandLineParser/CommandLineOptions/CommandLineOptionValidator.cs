using System.Globalization;
using System.Runtime.InteropServices;

namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

public class CommandLineOptionValidator : ICommandLineOptionValidator
{
    public int ValidateInterval(Dictionary<string, string> options)
    {
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

        return interval;
    }

    public DateTime ValidateExecutionDate(Dictionary<string, string> options)
    {
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

        return executionDate;
    }

    public string ValidateOutputFolder(Dictionary<string, string> options)
    {
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

        return outputFolder;
    }

    public string ValidateTimeZone(Dictionary<string, string> options)
    {
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

        return timeZone;
    }
}
