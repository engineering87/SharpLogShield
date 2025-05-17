// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.Logging;
using SharpLogShield.Logging;

namespace SharpLogShield.Providers
{
    /// <summary>
    /// A logger decorator that intercepts log messages and applies masking to sensitive data
    /// before passing them to the underlying logger.
    /// </summary>
    public class SharpLogShieldLogger : ILogger
    {
        private readonly ILogger _innerLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpLogShieldLogger"/> class.
        /// </summary>
        /// <param name="innerLogger">The original logger to wrap and delegate to after masking.</param>
        public SharpLogShieldLogger(ILogger innerLogger)
        {
            _innerLogger = innerLogger;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// Delegates to the inner logger's implementation.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope with.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            _innerLogger.BeginScope(state);

        /// <summary>
        /// Checks if the given log level is enabled.
        /// Delegates to the inner logger's implementation.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns><c>true</c> if logging is enabled at the specified level; otherwise, <c>false</c>.</returns>
        public bool IsEnabled(LogLevel logLevel) =>
            _innerLogger.IsEnabled(logLevel);

        /// <summary>
        /// Logs a message after masking any sensitive data it may contain.
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">The event ID associated with the log.</param>
        /// <param name="state">The entry to be written.</param>
        /// <param name="exception">The exception related to this log entry.</param>
        /// <param name="formatter">Function to create a string message from the state and exception.</param>
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