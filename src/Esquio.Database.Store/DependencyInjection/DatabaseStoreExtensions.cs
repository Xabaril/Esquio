using Esquio.Abstractions;
using Esquio.Database.Store;
using Esquio.Database.Store.Diagnostics;
using Esquio.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class DatabaseStoreExtensions
    {
        /// <summary>
        /// Add Esquio configuration using distributed store that connect with HTTP API on any 
        /// Esquio UI deployment.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="connectionString">The Esquio database connection string to use.</param>
        /// <param name="configurer">The action to configure default settings for internal Entity Framework Context.[Optional].</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddDatabaseStore(this IEsquioBuilder builder, string connectionString)
        {
            builder.Services.AddSingleton<EsquioDatabaseStoreDiagnostics>();

            builder.Services
                .AddScoped<IRuntimeFeatureStore, EsquioDatabaseStore>(sp =>
                {
                    return new EsquioDatabaseStore(
                        connectionString,
                        sp.GetRequiredService<EsquioDatabaseStoreDiagnostics>());
                });

            return builder;
        }
    }
}
