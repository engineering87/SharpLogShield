using Microsoft.Extensions.Logging;
using SharpLogShield.Providers;

namespace SharpLogShield.Factories
{
    public sealed class SharpLogShieldFactory : ILoggerFactory
    {
        private readonly ILoggerFactory _innerFactory;

        public SharpLogShieldFactory(ILoggerFactory innerFactory)
        {
            _innerFactory = innerFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var innerLogger = _innerFactory.CreateLogger(categoryName);
            return new SharpLogShieldLogger(innerLogger);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            _innerFactory.AddProvider(provider);
        }

        public void Dispose()
        {
            _innerFactory.Dispose();
        }
    }
}
