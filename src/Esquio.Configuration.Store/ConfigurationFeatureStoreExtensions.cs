using Esquio.Abstractions;
using Esquio.Configuration.Store;
using Esquio.Configuration.Store.Configuration;
using Esquio.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationFeatureStoreExtensions
    {
        private const string DefaultSectionName = "Esquio";

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
