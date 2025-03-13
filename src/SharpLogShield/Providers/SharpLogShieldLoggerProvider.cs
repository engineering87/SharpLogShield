using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace SharpLogShield.Providers
{
    [ProviderAlias("SharpLogShield")]
    public sealed class SharpLogShieldLoggerProvider : ILoggerProvider
    {
        private readonly ILoggerProvider _innerLoggerProvider;
        private readonly ConcurrentDictionary<string, SharpLogShieldLogger> _loggers = new();

        public SharpLogShieldLoggerProvider(ILoggerProvider loggerProvider)
        {
            _innerLoggerProvider = loggerProvider;
        }

        //public ILogger CreateLogger(string categoryName)
        //{
        //    var innerLogger = _loggerFactory.CreateLogger(categoryName);
        //    return new SharpLogShieldLogger(categoryName, innerLogger);
        //}

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name =>
            {
                var innerLogger = _innerLoggerProvider.CreateLogger(name);
                return new SharpLogShieldLogger(name, innerLogger);
            });
        }

        public void Dispose() => _loggers.Clear();
    }
}
