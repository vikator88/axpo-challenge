namespace AxpoChallenge.Application.Services;

public interface IPerformanceLoggingService
{
    Task LogExecutionTimeAsync(Func<Task> action, string actionName);
    Task<T> LogExecutionTimeAsync<T>(Func<Task<T>> action, string actionName);

    T LogExecutionTime<T>(Func<T> action, string actionName);

    void LogExecutionTime(Action action, string actionName);
}
