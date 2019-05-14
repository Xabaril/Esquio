using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder AddEntityFrameworkCoreStore(this IEsquioBuilder builder, Action<StoreOptions> configurer)
        {
            var options = new StoreOptions();
            var setup = configurer ?? (opts => { });
            setup(options);

            builder.Services.AddDbContextPool<StoreDbContext>(optionsAction =>
            {
                options.ConfigureDbContext?.Invoke(optionsAction);
            });

            builder.Services.AddScoped<IFeatureStore, EntityFrameworkCoreFeaturesStore>();

            return builder;
        }
    }
}
