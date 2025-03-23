using System;
using System.Threading;
using System.Threading.Tasks;
using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Application.UseCases.ExportPowerTrades;
using AxpoChallenge.Infrastructure.Configuration.CommandLineParser;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AxpoChallengeWorker : BackgroundService
{
    private readonly ILogger<AxpoChallengeWorker> _logger;
    private readonly IAxpoChallengeOptions _axpoChallengeOptions;
    private readonly IExportPowerTradesUseCase _exportPowerTradesUseCase;

    public AxpoChallengeWorker(
        IExportPowerTradesUseCase exportPowerTradesUseCase,
        IAxpoChallengeOptions axpoChallengeOptions,
        ILogger<AxpoChallengeWorker> logger
    )
    {
        _logger = logger;
        _axpoChallengeOptions = axpoChallengeOptions;
        _exportPowerTradesUseCase = exportPowerTradesUseCase;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"Worker started. Executing every {_axpoChallengeOptions.ExecutionIntervalMinutes} minutes."
        );

        int iteration = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            // Option 1: Run the use case directly in the main loop (blocking execution)
            /*
            try
            {
                await ExecuteUseCaseAsync();
                _logger.LogInformation("Process completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing process.");
            }

            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);

            iteration++;
            */

            // Option 2: Launch the use case as a background task (non-blocking)

            int currentIteration = ++iteration; // Capture the current iteration for use in the currnt task.
            _logger.LogInformation($"Iteration {currentIteration} started.");

            _ = Task.Run(
                async () =>
                {
                    try
                    {
                        await _exportPowerTradesUseCase.ExecuteAsync(
                            _axpoChallengeOptions.ExecutionDate.AddDays(1), // Execute use case for day-ahead
                            _axpoChallengeOptions.CsvOutputFolder
                        );
                        _logger.LogInformation($"Iteration {currentIteration} completed."); // use currentIteration instead iteration to ensure the correct value is logged.
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error executing iteration {currentIteration}.");
                    }
                },
                stoppingToken
            );

            await Task.Delay(
                TimeSpan.FromMinutes(_axpoChallengeOptions.ExecutionIntervalMinutes),
                stoppingToken
            );
        }
    }
}

/*
 Why is the second option (background task) better?

 1. Avoids blocking the loop: In the first option, if the use case takes longer than the interval, the next execution will be delayed.
    - This can cause drift and prevent timely executions.
  
 2. Ensures periodic execution: The second option always triggers the execution at the correct interval, even if the previous one is still running.
  
 3. Parallel execution: If needed, multiple executions can overlap without delaying the next scheduled run.
  
 5. Fault tolerance: If an execution fails, the loop continues running and schedules the next one as expected.
  
 The main risk is that if a task takes too long, multiple executions could overlap, leading to resource contention or performance issues.
 For this challenge, it won't be a problem because only use two resources: An API to fetch data and a CSV file to write the results with different names, depending on the time it was executed.
*/
