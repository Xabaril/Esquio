using System.Reflection;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, string connectionString, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure<StoreOptions>(setup);

            return services.AddEntityFrameworkSqlServer(connectionString);
        }

        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<StoreDbContext>(setup =>
                {
                    setup.UseSqlServer(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    });
                });
        }
    }
}
