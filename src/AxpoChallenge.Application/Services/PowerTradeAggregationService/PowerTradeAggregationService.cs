using System.Data.Common;
using AxpoChallenge.Application.Configuration;
using AxpoChallenge.Domain.Entities;
using AxpoChallenge.Domain.ValueObjects;

namespace AxpoChallenge.Application.Services
{
    public class PowerTradeAggregationService : IPowerTradeAggregationService
    {
        private readonly IAxpoChallengeOptions _options;

        public PowerTradeAggregationService(IAxpoChallengeOptions options)
        {
            _options = options;
        }

        public IEnumerable<AggregatedPowerPosition> AggregateTrades(
            IEnumerable<PowerTradeEntity> trades
        )
        {
            // Check if all power trades are for the same date
            if (trades.Select(trade => trade.Date).Distinct().Count() > 1)
            {
                throw new ArgumentException("All power trades must be for the same date.");
            }

            // Check if all power trades have the same number of periods
            if (trades.Select(trade => trade.Periods.Length).Distinct().Count() > 1)
            {
                throw new ArgumentException(
                    "All power trades must have the same number of periods."
                );
            }

            // Convert local date (time 00:00) to UTC time. This conversion has in mind the daylight saving time changes if the local time zone is UTC + an entire hour AND the change time is not at 00:00.
            // This works in the context of the challenge, where the local time is always Berlin time.
            // This also works in all those cases where the local time zone is UTC + an entire hour AND the change time is not at 00:00. (In general, any time zone in Europe will work correctly)
            // This solution does not work for time zones that change to summer or winter time at 00:00. (For example, Azores time zone). In this case, the conversion should be done with the time zone information in a more complex way.

            // Get the local date of the trades
            DateTime localDate = DateTime.SpecifyKind(
                trades.First().Date,
                DateTimeKind.Unspecified // The date is not UTC. The time zone is setted using the options. Don't trust the server time zone.
            );

            // Convert the local date to UTC
            DateTime dateTimeUtcBase = TimeZoneInfo.ConvertTimeToUtc(
                localDate,
                TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZone) // Time zone in options
            );

            // At this point, base hour is UTC. Days with 23 or 25 periods due to daylight saving time will work correctly with the previous considerations.

            var aggregatedPositions = new Dictionary<int, AggregatedPowerPosition>();

            foreach (PowerTradeEntity trade in trades)
            {
                foreach (PowerPeriodValueObject period in trade.Periods)
                {
                    // Starting from local date 00:00 (but already converted to UTC), each period is 1 hour long.
                    DateTime currentPeriodDateTimeUtc = dateTimeUtcBase.AddHours(period.Period - 1); // Period Id is 1-based.

                    if (!aggregatedPositions.ContainsKey(period.Period))
                    {
                        aggregatedPositions[period.Period] = new AggregatedPowerPosition(
                            currentPeriodDateTimeUtc
                        );
                    }

                    aggregatedPositions[period.Period].AddVolume(period.Volume);
                }
            }

            return aggregatedPositions.Values;
        }
    }
}
