// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpLogShield.Factories;

namespace SharpLogShield.Extensions
{
    /// <summary>
    /// Extension methods for integrating SharpLogShield into the logging builder.
    /// </summary>
    public static class SharpLogShieldExtensions
    {
        /// <summary>
        /// Replaces the default <see cref="ILoggerFactory"/> with a <see cref="SharpLogShieldFactory"/>
        /// that wraps the original factory to provide logging protections.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure.</param>
        /// <returns>The updated <see cref="ILoggingBuilder"/> instance.</returns>
        public static ILoggingBuilder AddSharpLogShieldLogging(this ILoggingBuilder builder)
        {
            // Replace the default ILoggerFactory with a custom SharpLogShieldFactory
            builder.Services.Replace(ServiceDescriptor.Singleton<ILoggerFactory>(sp =>
            {
                // Retrieve the existing logging components from the service provider
                var providers = sp.GetServices<ILoggerProvider>();
                var loggerOptions = sp.GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>();
                var factoryOptions = sp.GetRequiredService<IOptions<LoggerFactoryOptions>>();

                // Create the original logger factory with default providers and options
                var originalFactory = new LoggerFactory(providers, loggerOptions, factoryOptions);

                // Return the SharpLogShieldFactory wrapping the original factory
                return new SharpLogShieldFactory(originalFactory);
            }));

            return builder;
        }
    }
}