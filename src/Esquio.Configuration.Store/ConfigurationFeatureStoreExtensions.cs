using Esquio.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace Esquio.Configuration.Store
{
    public static class ConfigurationFeatureStoreExtensions
    {
        private const string DefaultSectionName = "Esquio";

        public static IEsquioBuilder AddConfigurationStore(this IEsquioBuilder builder, IConfiguration configuration)
        {
            var libraries = DependencyContext
                .Default
                .CompileLibraries
                .Select(c => c.Name);

            builder.Services.AddSingleton<IFeatureStore>(sp =>
                new ConfigurationFeatureStore(
                    configuration.GetSection(DefaultSectionName),
                    null,
                    libraries));

            return builder;
        }
    }
}
