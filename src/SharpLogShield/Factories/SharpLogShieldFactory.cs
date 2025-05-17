// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.Logging;
using SharpLogShield.Providers;

namespace SharpLogShield.Factories
{
    /// <summary>
    /// A custom implementation of <see cref="ILoggerFactory"/> that wraps another factory
    /// and returns loggers decorated with SharpLogShield functionality.
    /// </summary>
    public sealed class SharpLogShieldFactory : ILoggerFactory
    {
        private readonly ILoggerFactory _innerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpLogShieldFactory"/> class.
        /// </summary>
        /// <param name="innerFactory">The original <see cref="ILoggerFactory"/> to wrap.</param>
        public SharpLogShieldFactory(ILoggerFactory innerFactory)
        {
            _innerFactory = innerFactory;
        }

        /// <summary>
        /// Creates a logger with the specified category name, wrapping the original logger
        /// with a <see cref="SharpLogShieldLogger"/> to provide additional features.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>A <see cref="SharpLogShieldLogger"/> instance wrapping the original logger.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            var innerLogger = _innerFactory.CreateLogger(categoryName);
            return new SharpLogShieldLogger(innerLogger);
        }

        /// <summary>
        /// Adds a logging provider to the inner logger factory.
        /// </summary>
        /// <param name="provider">The <see cref="ILoggerProvider"/> to add.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            _innerFactory.AddProvider(provider);
        }

        /// <summary>
        /// Disposes the inner logger factory and releases its resources.
        /// </summary>
        public void Dispose()
        {
            _innerFactory.Dispose();
        }
    }
}