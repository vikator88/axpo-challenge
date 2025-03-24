namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser
{
    public interface ICommandLineParser
    {
        CommandLineOptions Parse(string[] args);
    }
}
