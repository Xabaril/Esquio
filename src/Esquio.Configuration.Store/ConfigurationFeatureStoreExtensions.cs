using Esquio.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Esquio.Configuration.Store
{
    public static class ConfigurationFeatureStoreExtensions
    {
        private const string DefaultSectionName = "Esquio";

        public static IEsquioBuilder AddConfigurationStore(this IEsquioBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton<IFeatureStore>(sp =>
                new ConfigurationFeatureStore(
                    configuration.GetSection(DefaultSectionName), null));

            return builder;
        }
    }
}
