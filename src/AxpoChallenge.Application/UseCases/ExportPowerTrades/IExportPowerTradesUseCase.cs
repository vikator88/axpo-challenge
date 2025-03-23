namespace AxpoChallenge.Application.UseCases.ExportPowerTrades
{
    public interface IExportPowerTradesUseCase
    {
        /// <summary>
        /// Export power trades to a CSV file for a given date
        /// and save it to the specified destination folder.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        Task ExecuteAsync(DateTime date, string destinationFolder);
    }
}
