using Microsoft.Extensions.Logging;
using SharpLogShield.Logging;

namespace SharpLogShield.Providers
{
    public class SharpLogShieldLogger : ILogger
    {
        private readonly ILogger _innerLogger;

        public SharpLogShieldLogger(ILogger innerLogger)
        {
            _innerLogger = innerLogger;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            _innerLogger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) =>
            _innerLogger.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (formatter != null)
            {
                string originalMessage = formatter(state, exception);
                string maskedMessage = LogMasker.MaskSensitiveData(originalMessage);

                _innerLogger.Log(logLevel, eventId, state, exception, (s, ex) => maskedMessage);
            }
        }
    }
}
