namespace AxpoChallenge.Application.Services
{
    public interface ICsvExportService
    {
        /// <summary>
        /// Export data to a CSV file and save it to the specified destination path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fullDestinationPath">file destination including filename</param>
        /// <returns></returns>
        Task ExportAsync<T>(IEnumerable<T> data, string fullDestinationPath);
    }
}
