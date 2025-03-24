using AxpoChallenge.Application.Configuration;

namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser
{
    public class CommandLineOptions : IAxpoChallengeOptions
    {
        public int ExecutionIntervalMinutes { get; private set; }
        public DateTime ExecutionDate { get; private set; }
        public string CsvOutputFolder { get; private set; }
        public string TimeZone { get; private set; }

        public CommandLineOptions(
            int executionIntervalMinutes,
            DateTime executionDate,
            string csvOutputFolder,
            string timeZone
        )
        {
            ExecutionIntervalMinutes = executionIntervalMinutes;
            ExecutionDate = executionDate;
            CsvOutputFolder = csvOutputFolder;
            TimeZone = timeZone;
        }
    }
}
