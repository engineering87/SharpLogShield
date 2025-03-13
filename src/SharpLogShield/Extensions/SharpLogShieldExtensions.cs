using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharpLogShield.Providers;

namespace SharpLogShield.Extensions
{
    public static class SharpLogShieldExtensions
    {
        public static ILoggingBuilder AddSharpLogShieldLogging(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                var innerProvider = serviceProvider.GetRequiredService<ILoggerProvider>();
                return new SharpLogShieldLoggerProvider(innerProvider);
            });

            //builder.Services.AddSingleton<ILoggerProvider, SharpLogShieldLoggerProvider>();

            return builder;
        }
    }
}
