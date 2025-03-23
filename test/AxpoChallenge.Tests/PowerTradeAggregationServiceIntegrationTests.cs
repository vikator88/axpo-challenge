using System.Threading.Tasks;
using Axpo;
using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Application.Services;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Infrastructure.Configuration.CommandLineParser;
using AxpoChallenge.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace AxpoChallenge.Tests;

[TestFixture]
public class PowerTradeAggregationServiceIntegrationTests
{
    private PowerTradeRepository _powerTradeRepository;

    private IAxpoChallengeOptions options;

    [SetUp]
    public void SetUp()
    {
        // Mock the logger
        var logger = new Mock<ILogger<PowerTradeRepository>>().Object;

        // Create instance of PowerTradeRepository with the actual service as dependency
        _powerTradeRepository = new PowerTradeRepository(new PowerService(), logger);

        // create test options
        options = new CommandLineOptionsDTOBuilder()
            .WithExecutionIntervalMinutes(1)
            .WithCsvOutputFolder(Directory.GetCurrentDirectory())
            .WithTimeZone("Central European Standard Time")
            .Build();
    }

    [Test]
    public async Task AggregateTrades_ShouldReturnAggregatedPowerPositions()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a new instance of the service
        var service = new PowerTradeAggregationService(options);

        var date = new DateTime(2024, 10, 27);

        /***********/
        /*   ACT   */
        /***********/

        IEnumerable<PowerTradeEntity> _powerTrades =
            await _powerTradeRepository.GetTradesByDateAsync(date);

        IEnumerable<AggregatedPowerPosition> aggregatedTrades = service.AggregateTrades(
            _powerTrades
        );

        /***********/
        /*  ASSERT */
        /***********/
        Assert.That(aggregatedTrades, Is.Not.Null);
        Assert.That(aggregatedTrades.Count(), Is.EqualTo(_powerTrades.First().Periods.Length));

        // Verify that the aggregated trades have the correct volume
        for (int i = 0; i < _powerTrades.First().Periods.Length; i++)
        {
            Assert.That(
                aggregatedTrades.ElementAt(i).Volume,
                Is.EqualTo(_powerTrades.Sum(x => x.Periods[i].Volume))
            );
        }
    }

    [Test]
    public async Task AggregateTrades_ShouldHaveOneHourDifference_WhenTwoFollowingDaysAreAggregated_OnWinterDaylighgSavingDate()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a test date for checking winter daylight saving time
        var date1 = new DateTime(2024, 10, 27);
        var date2 = new DateTime(2024, 10, 28);

        /***********/
        /*   ACT   */
        /***********/

        Task<IEnumerable<PowerTradeEntity>> result1 = _powerTradeRepository.GetTradesByDateAsync(
            date1
        );
        Task<IEnumerable<PowerTradeEntity>> result2 = _powerTradeRepository.GetTradesByDateAsync(
            date2
        );

        // Wait for both tasks to complete
        await Task.WhenAll(result1, result2);

        var service = new PowerTradeAggregationService(options);
        IEnumerable<AggregatedPowerPosition> aggregatedTrades1 = service.AggregateTrades(
            await result1
        );
        IEnumerable<AggregatedPowerPosition> aggregatedTrades2 = service.AggregateTrades(
            await result2
        );

        /************/
        /*  ASSERT  */
        /************/

        // Verify that the result is not null
        Assert.NotNull(aggregatedTrades1);
        Assert.NotNull(aggregatedTrades2);

        // Verify that the result is not empty
        Assert.IsTrue(aggregatedTrades1.Any());
        Assert.IsTrue(aggregatedTrades2.Any());

        // Verify that the first day has 25 trades
        Assert.That(aggregatedTrades1.Count, Is.EqualTo(25));

        // Verify that the second day has 24 trades
        Assert.That(aggregatedTrades2.Count, Is.EqualTo(24));

        // Verify that the last trade of the first day is one hour before the first trade of the second day
        Assert.That(
            aggregatedTrades1.Last().DateTimeUtc.AddHours(1),
            Is.EqualTo(aggregatedTrades2.First().DateTimeUtc)
        );
    }

    [Test]
    public async Task AggregateTrades_ShouldHaveOneHourDifference_WhenTwoFollowingDaysAreAggregated_OnSummerDaylighgSavingDate()
    {
        /***********/
        /* ARRANGE */
        /***********/
        // Define AxpoChallengeOptions to run the service
        CommandLineOptionsDTO options = new CommandLineOptionsDTOBuilder()
            .WithExecutionIntervalMinutes(1)
            .WithCsvOutputFolder(Directory.GetCurrentDirectory())
            .WithTimeZone("Central European Standard Time")
            .Build();

        // Create a test date for checking summer daylight saving time
        var date1 = new DateTime(2025, 3, 30);
        var date2 = new DateTime(2025, 3, 31);

        /***********/
        /*   ACT   */
        /***********/

        Task<IEnumerable<PowerTradeEntity>> result1 = _powerTradeRepository.GetTradesByDateAsync(
            date1
        );
        Task<IEnumerable<PowerTradeEntity>> result2 = _powerTradeRepository.GetTradesByDateAsync(
            date2
        );

        // Wait for both tasks to complete
        await Task.WhenAll(result1, result2);

        var service = new PowerTradeAggregationService(options);
        IEnumerable<AggregatedPowerPosition> aggregatedTrades1 = service.AggregateTrades(
            await result1
        );
        IEnumerable<AggregatedPowerPosition> aggregatedTrades2 = service.AggregateTrades(
            await result2
        );

        /************/
        /*  ASSERT  */
        /************/

        // Verify that the result is not null
        Assert.NotNull(aggregatedTrades1);
        Assert.NotNull(aggregatedTrades2);

        // Verify that the result is not empty
        Assert.IsTrue(aggregatedTrades1.Any());
        Assert.IsTrue(aggregatedTrades2.Any());

        // Verify that the first day has 25 trades
        Assert.That(aggregatedTrades1.Count, Is.EqualTo(23));

        // Verify that the second day has 24 trades
        Assert.That(aggregatedTrades2.Count, Is.EqualTo(24));

        // Verify that the last trade of the first day is one hour before the first trade of the second day
        Assert.That(
            aggregatedTrades1.Last().DateTimeUtc.AddHours(1),
            Is.EqualTo(aggregatedTrades2.First().DateTimeUtc)
        );
    }
}
