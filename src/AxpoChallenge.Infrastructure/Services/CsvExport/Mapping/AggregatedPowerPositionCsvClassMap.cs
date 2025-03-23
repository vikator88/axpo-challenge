using System.Globalization;
using AxpoChallenge.Domain.Entities;
using CsvHelper.Configuration;

namespace AxpoChallenge.Infrastructure.Services.CsvExport.Mapping;

public class AggregatedPowerPositionCsvClassMap : ClassMap<AggregatedPowerPosition>
{
    public AggregatedPowerPositionCsvClassMap()
    {
        Map(m => m.DateTimeUtc)
            .Name("DateTimeUtc")
            .TypeConverterOption.Format("yyyy-MM-ddTHH:mm:ssZ");
        Map(m => m.Volume)
            .Name("Volume")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
    }
}
