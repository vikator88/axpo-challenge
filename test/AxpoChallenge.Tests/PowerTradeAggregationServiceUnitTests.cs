using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Application.Services;
using AxpoChallenge.Domain.Builders;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Domain.ValueObjects;
using AxpoChallenge.Infrastructure.Configuration.CommandLineParser;

namespace AxpoChallenge.Tests;

[TestFixture]
public class PowerTradeAggregationServiceUnitTests
{
    // define a list of power trades to be used in the tests
    private List<PowerTradeEntity> _powerTrades;
    private IAxpoChallengeOptions options;

    [SetUp]
    public void SetUp()
    {
        // create 4 sets of 24 power period value objects
        PowerPeriodValueObject[] periods1 = new PowerPeriodValueObject[24];
        PowerPeriodValueObject[] periods2 = new PowerPeriodValueObject[24];
        PowerPeriodValueObject[] periods3 = new PowerPeriodValueObject[24];
        PowerPeriodValueObject[] periods4 = new PowerPeriodValueObject[24];

        // fill the power period value objects with random data
        for (int i = 0; i < 24; i++)
        {
            //Periods are 1-based
            periods1[i] = new PowerPeriodValueObject(i + 1, new Random().Next(-50, 50));
            periods2[i] = new PowerPeriodValueObject(i + 1, new Random().Next(-50, 50));
            periods3[i] = new PowerPeriodValueObject(i + 1, new Random().Next(-50, 50));
            periods4[i] = new PowerPeriodValueObject(i + 1, new Random().Next(-50, 50));
        }

        // create 4 power trade entities with the power period value objects
        PowerTradeEntity trade1 = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 21))
            .WithPeriods(periods1)
            .Build();

        PowerTradeEntity trade2 = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 21))
            .WithPeriods(periods2)
            .Build();

        PowerTradeEntity trade3 = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 21))
            .WithPeriods(periods3)
            .Build();

        PowerTradeEntity trade4 = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 21))
            .WithPeriods(periods4)
            .Build();

        // add the power trade entities to the list
        _powerTrades = new List<PowerTradeEntity> { trade1, trade2, trade3, trade4 };

        // Define AxpoChallengeOptions to run the service
        options = new CommandLineOptionsDTOBuilder()
            .WithExecutionIntervalMinutes(1)
            .WithExecutionDate(new DateTime(2025, 3, 21))
            .WithCsvOutputFolder(Directory.GetCurrentDirectory())
            .WithTimeZone("Central European Standard Time")
            .Build();
    }

    [Test]
    public void AggregateTrades_ShouldReturnAggregatedPowerPositions()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a new instance of the service
        var service = new PowerTradeAggregationService(options);

        /***********/
        /*   ACT   */
        /***********/
        IEnumerable<AggregatedPowerPosition> aggregatedTrades1 = service.AggregateTrades(
            _powerTrades
        );

        /***********/
        /*  ASSERT */
        /***********/
        Assert.That(aggregatedTrades1, Is.Not.Null);
        Assert.That(aggregatedTrades1.Count(), Is.EqualTo(24));

        // Verify that the aggregated trades have the correct volume
        for (int i = 0; i < 24; i++)
        {
            Assert.That(
                aggregatedTrades1.ElementAt(i).Volume,
                Is.EqualTo(_powerTrades.Sum(x => x.Periods[i].Volume))
            );
        }
    }

    [Test]
    public void AggregateTrades_ShouldThrowException_WhenTradesHaveDifferentNumberOfPeriods()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a new instance of the service
        var service = new PowerTradeAggregationService(options);

        // Copy the settedup trades in setup
        List<PowerTradeEntity> trades = new List<PowerTradeEntity>(_powerTrades);

        // Add a new trade with a different number of periods
        PowerPeriodValueObject[] periods = new PowerPeriodValueObject[23];
        for (int i = 0; i < 23; i++)
        {
            periods[i] = new PowerPeriodValueObject(i + 1, new Random().Next(-50, 50));
        }

        PowerTradeEntity trade = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 21))
            .WithPeriods(periods)
            .Build();

        trades.Add(trade);

        /***********/
        /*   ACT   */
        /***********/
        // Call the AggregateTrades method with the new trades
        // using a lambda to call the method and catch the exception
        void act() => service.AggregateTrades(trades);

        /***********/
        /*  ASSERT */
        /***********/
        // Verify that an exception is thrown
        Assert.That(act, Throws.Exception.TypeOf<ArgumentException>());
    }

    [Test]
    public void AggregateTrades_ShouldThrowException_WhenTradesHaveDifferentDates()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a new instance of the service
        var service = new PowerTradeAggregationService(options);

        // Copy the settedup trades in setup
        List<PowerTradeEntity> trades = new List<PowerTradeEntity>(_powerTrades);

        // Add a new trade with a different date

        PowerTradeEntity trade = new PowerTradeEntityBuilder()
            .WithTradeId(Guid.NewGuid().ToString())
            .WithTradeDate(new DateTime(2025, 3, 22))
            .WithPeriods(trades.First().Periods)
            .Build();

        trades.Add(trade);

        /***********/
        /*   ACT   */
        /***********/
        // Call the AggregateTrades method with the new trades
        // using a lambda to call the method and catch the exception
        void act() => service.AggregateTrades(trades);

        /***********/
        /*  ASSERT */
        /***********/
        // Verify that an exception is thrown
        Assert.That(act, Throws.Exception.TypeOf<ArgumentException>());
    }
}
