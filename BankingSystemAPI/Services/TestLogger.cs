

namespace BankingSystemAPI.Services
{
    public class TestLogger : ILoggingService
    {
        private readonly ILogger _logger;

        public TestLogger(ILogger<TestLogger> logger)
        {
                _logger = logger;
        }

        public void LogInformation(string message) {
            _logger.LogInformation(message);
        }
    }
}
