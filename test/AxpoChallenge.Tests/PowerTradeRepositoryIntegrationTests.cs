using Axpo;
using AxpoChallenge.Infrastructure;

namespace AxpoChallenge.Tests;

[TestFixture]
public class PowerTradeRepositoryIntegrationTests
{
    private PowerTradeRepository _powerTradeRepository;

    [SetUp]
    public void SetUp()
    {
        // Create instance of PowerTradeRepository with the actual service as dependency
        _powerTradeRepository = new PowerTradeRepository(new PowerService());
    }

    [Test]
    public async Task GetTradesAsync_ShouldReturnValidTrades_WhenServiceIsCalled()
    {
        /***********/
        /* ARRANGE */
        /***********/

        // Create a test date
        var date = new DateTime(2025, 3, 21);

        /***********/
        /*   ACT   */
        /***********/

        var result = await _powerTradeRepository.GetTradesByDateAsync(date);

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
