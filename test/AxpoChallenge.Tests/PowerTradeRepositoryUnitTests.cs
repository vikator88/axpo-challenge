using Axpo;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace AxpoChallenge.Tests;

[TestFixture]
public class PowerTradeRepositoryUnitTests
{
    private Mock<IPowerService> _mockPowerService;
    private PowerTradeRepository _powerTradeRepository;

    [SetUp]
    public void SetUp()
    {
        // Create mock for IPowerService
        _mockPowerService = new Mock<IPowerService>();

        // Mock the logger
        var logger = new Mock<ILogger<PowerTradeRepository>>().Object;

        // Create instance of PowerTradeRepository with the mock as dependency
        _powerTradeRepository = new PowerTradeRepository(_mockPowerService.Object, logger);
    }

    [Test]
    public async Task GetTradesAsync_ShouldReturnMappedPowerTrades()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a test date and a list of mock trades
        var testDate = new DateTime(2025, 3, 21);

        // Create a list of mock PowerTrade for the requested date
        List<PowerTrade> mockTrades = new List<PowerTrade>
        {
            PowerTrade.Create(testDate, 24),
            PowerTrade.Create(testDate, 24)
        };

        // Fill the mock trades with some data
        foreach (PowerTrade trade in mockTrades)
        {
            for (int i = 0; i < trade.Periods.Length; i++)
            {
                // set random integer value for volume
                int volume = new Random().Next(-50, 50);
                trade.Periods[i].SetVolume(volume);
            }
        }

        // Setup the mock GetTradesAsync method to return the mock trades
        _mockPowerService.Setup(x => x.GetTradesAsync(testDate)).ReturnsAsync(mockTrades);

        /***********/
        /*   ACT   */
        /***********/

        // Call method GetTradesByDateAsync from repository
        IEnumerable<PowerTradeEntity> result = await _powerTradeRepository.GetTradesByDateAsync(
            testDate
        );

        /************/
        /*  ASSERT  */
        /************/

        // Verify that GetTradesAsync method in external service was called once
        _mockPowerService.Verify(x => x.GetTradesAsync(testDate), Times.Once);

        // Verify that the result is not null
        Assert.IsNotNull(result);

        // Verify that the result is of type IEnumerable<PowerTradeDomain>
        Assert.IsInstanceOf<IEnumerable<PowerTradeEntity>>(result);

        // Verify that the result contains the same number of trades as the mock trades
        Assert.That(result.Count(), Is.EqualTo(mockTrades.Count));

        // Verify that the result contains the same volume values as the mock trades
        for (int i = 0; i < mockTrades.Count; i++)
        {
            for (int j = 0; j < mockTrades[i].Periods.Length; j++)
            {
                Assert.That(
                    result.ElementAt(i).Periods[j].Volume,
                    Is.EqualTo(mockTrades[i].Periods[j].Volume)
                );
            }
        }
    }

    [Test]
    public async Task GetTradesAsync_ShouldReturnEmptyList_WhenNoTradesFound()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a test date and a list of mock trades
        var testDate = new DateTime(2025, 3, 21);

        // Setup the mock GetTradesAsync method to return an empty list
        _mockPowerService
            .Setup(x => x.GetTradesAsync(testDate))
            .ReturnsAsync(new List<PowerTrade>());

        /***********/
        /*   ACT   */
        /***********/

        // Call method GetTradesByDateAsync from repository
        IEnumerable<PowerTradeEntity> result = await _powerTradeRepository.GetTradesByDateAsync(
            testDate
        );

        /************/
        /*  ASSERT  */
        /*************/

        // Verify that GetTradesAsync method in external service was called once
        _mockPowerService.Verify(x => x.GetTradesAsync(testDate), Times.Once);

        // Verify that the result is not null
        Assert.IsNotNull(result);

        // Verify that the result is of type IEnumerable<PowerTradeDomain>
        Assert.IsInstanceOf<IEnumerable<PowerTradeEntity>>(result);

        // Verify that the result is an empty list
        Assert.That(result.Count(), Is.EqualTo(0));
    }
}
