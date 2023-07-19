namespace BankingSystemAPI.Services
{
    public interface ILoggingService
    {
        /// <summary>
        /// Logs information
        /// </summary>
        /// <param name="message"></param>
        void LogInformation(string message);

        /// <summary>
        /// Logs warning
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(string message);

        /// <summary>
        /// Logs error
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
}
