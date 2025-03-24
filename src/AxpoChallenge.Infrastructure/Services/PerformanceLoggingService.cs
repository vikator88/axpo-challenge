using System.Diagnostics;
using AxpoChallenge.Application.Services;
using Microsoft.Extensions.Logging;

namespace AxpoChallenge.Infrastructure.Services
{
    public class PerformanceLoggingService : IPerformanceLoggingService
    {
        private readonly ILogger<PerformanceLoggingService> _logger;

        public PerformanceLoggingService(ILogger<PerformanceLoggingService> logger)
        {
            _logger = logger;
        }

        public async Task LogExecutionTimeAsync(Func<Task> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await action();
            }
            finally
            {
                // If a method throws an exception, the finally block will run before the exception is propagated up the call stack.
                stopwatch.Stop();
                _logger.LogInformation(
                    string.Format(
                        "Action {0} took {1}ms",
                        actionName,
                        stopwatch.ElapsedMilliseconds
                    )
                );
            }
        }

        public async Task<T> LogExecutionTimeAsync<T>(Func<Task<T>> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = await action();
                return result;
            }
            finally
            {
                // If a method throws an exception, the finally block will run before the exception is propagated up the call stack.
                stopwatch.Stop();
                _logger.LogInformation(
                    string.Format(
                        "Action {0} took {1}ms",
                        actionName,
                        stopwatch.ElapsedMilliseconds
                    )
                );
            }
        }

        public T LogExecutionTime<T>(Func<T> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = action();
                return result;
            }
            finally
            {
                // If a method throws an exception, the finally block will run before the exception is propagated up the call stack.
                stopwatch.Stop();
                _logger.LogInformation(
                    string.Format(
                        "Action {0} took {1}ms",
                        actionName,
                        stopwatch.ElapsedMilliseconds
                    )
                );
            }
        }

        public void LogExecutionTime(Action action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                // If a method throws an exception, the finally block will run before the exception is propagated up the call stack.
                stopwatch.Stop();
                _logger.LogInformation(
                    string.Format(
                        "Action {0} took {1}ms",
                        actionName,
                        stopwatch.ElapsedMilliseconds
                    )
                );
            }
        }
    }
}
