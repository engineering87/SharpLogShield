using Microsoft.Extensions.Logging;

namespace SharpLogShield.Providers
{
    public class SharpLogShieldLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly ILogger _innerLogger;

        public SharpLogShieldLogger(string categoryName, ILogger innerLogger)
        {
            _categoryName = categoryName;
            _innerLogger = innerLogger;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => _innerLogger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                string originalMessage = formatter(state, exception);
                string maskedMessage = Logging.LogMasker.MaskSensitiveData(originalMessage);

                _innerLogger.Log(logLevel, eventId, maskedMessage, exception);
            }
        }
    }
}
