using Axpo;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace AxpoChallenge.Tests;

[TestFixture]
public class PowerTradeRepositoryIntegrationTests
{
    private PowerTradeRepository _powerTradeRepository;

    [SetUp]
    public void SetUp()
    {
        // Mock the logger
        var logger = new Mock<ILogger<PowerTradeRepository>>().Object;
        // Create instance of PowerTradeRepository with the actual service as dependency
        _powerTradeRepository = new PowerTradeRepository(new PowerService(), logger);
    }

    [Test]
    public async Task GetTradesAsync_ShouldReturnValidTrades_WhenServiceIsCalled()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a test date
        var date = new DateTime(2025, 3, 31);

        /***********/
        /*   ACT   */
        /***********/

        IEnumerable<PowerTradeEntity> result = await _powerTradeRepository.GetTradesByDateAsync(
            date
        );

        /************/
        /*  ASSERT  */
        /************/

        // Verify that the result is not null
        Assert.NotNull(result);

        // Verify that the result is not empty
        Assert.IsTrue(result.Any());

        // Verify that the result contains the expected domain value
        Assert.That(result.First().Date, Is.EqualTo(date));
    }
}
