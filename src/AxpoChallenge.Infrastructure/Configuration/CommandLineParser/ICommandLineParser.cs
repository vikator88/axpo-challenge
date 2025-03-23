namespace AxpoChallenge.Infrastructure.Configuration.CommandLineParser
{
    public interface ICommandLineParser
    {
        CommandLineOptionsDTO Parse(string[] args);
    }
}
