using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpLogShield.Factories;

namespace SharpLogShield.Extensions
{
    public static class SharpLogShieldExtensions
    {
        public static ILoggingBuilder AddSharpLogShieldLogging(this ILoggingBuilder builder)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<ILoggerFactory>(sp =>
            {
                var providers = sp.GetServices<ILoggerProvider>();
                var loggerOptions = sp.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
                var factoryOptions = sp.GetRequiredService<IOptions<LoggerFactoryOptions>>();

                var originalFactory = new LoggerFactory(providers, loggerOptions, factoryOptions);

                return new SharpLogShieldFactory(originalFactory);
            }));

            return builder;
        }
    }
}
