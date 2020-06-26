using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkNpgsql(this IServiceCollection services, string connectionString, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure<StoreOptions>(setup);

            return services.AddEntityFrameworkNpgsql(connectionString);
        }

        public static IServiceCollection AddEntityFrameworkNpgsql(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<StoreDbContext>(setup =>
                {
                    setup.UseNpgsql(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    });
                });
        }
    }
}
