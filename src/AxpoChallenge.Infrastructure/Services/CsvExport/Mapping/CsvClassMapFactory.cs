using AxpoChallenge.Infrastructure.Services.CsvExport.Mapping;
using CsvHelper.Configuration;

public static class CsvClassMapFactory
{
    public static ClassMap Create<T>()
    {
        var classMapType = typeof(T).Name;

        switch (classMapType.ToLower())
        {
            case "aggregatedpowerposition":
                return new AggregatedPowerPositionCsvClassMap();
            default:
                return null;
        }
    }
}
