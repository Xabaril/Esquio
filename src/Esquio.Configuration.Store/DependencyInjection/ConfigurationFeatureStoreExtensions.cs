using Esquio.Abstractions;
using Esquio.Configuration.Store;
using Esquio.Configuration.Store.Configuration;
using Esquio.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class ConfigurationFeatureStoreExtensions
    {
        private const string DefaultSectionName = "Esquio";

        /// <summary>
        /// Add Esquio configuration using <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> when the products, features and toggles are configured.</param>
        /// <param name="key">The configuration section key to use.[Optional] default value is Esquio.</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddConfigurationStore(this IEsquioBuilder builder, IConfiguration configuration, string key = DefaultSectionName)
        {
            builder.Services
                .AddOptions()
                .Configure<EsquioConfiguration>(configuration.GetSection(key))
                .AddScoped<IRuntimeFeatureStore, ConfigurationFeatureStore>();

            return builder;
        }
    }
}
