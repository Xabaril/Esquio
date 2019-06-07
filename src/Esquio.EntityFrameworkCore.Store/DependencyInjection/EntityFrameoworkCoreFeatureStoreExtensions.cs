using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class EntityFrameoworkCoreFeatureStoreExtensions
    {
        /// <summary>
        /// Add Esquio configuration using Entity Framework Core.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="configurer">The action to configure default settings for internal Entity Framework Context.[Optional].</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddEntityFrameworkCoreStore(this IEsquioBuilder builder, Action<StoreOptions> configurer = null)
        {
            var options = new StoreOptions();
            configurer?.Invoke(options);

            builder.Services.AddDbContextPool<StoreDbContext>(optionsAction =>
            {
                options.ConfigureDbContext?.Invoke(optionsAction);
            });

            builder.Services.AddScoped<IRuntimeFeatureStore, EntityFrameworkCoreFeaturesStore>();

            return builder;
        }
    }
}
