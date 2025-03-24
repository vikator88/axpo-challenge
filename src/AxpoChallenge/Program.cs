using Axpo;
using AxpoChallenge;
using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Application.Repositories;
using AxpoChallenge.Application.Services;
using AxpoChallenge.Application.UseCases.ExportPowerTrades;
using AxpoChallenge.Infrastructure.Configuration.CommandLineParser;
using AxpoChallenge.Infrastructure.Repositories;
using AxpoChallenge.Infrastructure.Services;
using AxpoChallenge.Infrastructure.Services.CsvExport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Dependency Injection
builder.Services.AddSingleton<ICommandLineParser, CommandLineParser>();
builder.Services.AddSingleton<IPowerService, PowerService>();
builder.Services.AddSingleton<IPowerTradeRepository, PowerTradeRepository>();
builder.Services.AddSingleton<ICsvExportService, CsvExportService>();
builder.Services.AddSingleton<IPowerTradeAggregationService, PowerTradeAggregationService>();
builder.Services.AddSingleton<IExportPowerTradesUseCase, ExportPowerTradesUseCase>();
builder.Services.AddSingleton<IPerformanceLoggingService, PerformanceLoggingService>();
builder.Services.AddSingleton<ICommandLineOptionValidator, CommandLineOptionValidator>();
var serviceProvider = builder.Services.BuildServiceProvider();

// Parse command line arguments
ICommandLineParser commandLineParser = serviceProvider.GetRequiredService<ICommandLineParser>();
CommandLineOptions commandLineOptions = commandLineParser.Parse(args);

// Print parsed configuration
Console.WriteLine("Axpo Coding Challenge!");
Console.WriteLine($"Execution interval: {commandLineOptions.ExecutionIntervalMinutes} minutes");
Console.WriteLine($"Output folder: {commandLineOptions.CsvOutputFolder}");
Console.WriteLine($"Time zone: {commandLineOptions.TimeZone}");

// Register CommandLineOptionsDTO as a singleton to be used by the AxpoChallenge Worker
builder.Services.AddSingleton<IAxpoChallengeOptions>(commandLineOptions);

// Register the AxpoChallenge Worker service
builder.Services.AddHostedService<AxpoChallengeWorker>();

// Build and execute the host
using var host = builder.Build();
Console.WriteLine("Press Ctrl + C to exit...");
await host.RunAsync();
