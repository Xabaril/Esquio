using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Host;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, string connectionString, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure<StoreOptions>(setup);

            return services.AddEntityFramework(connectionString);
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<StoreDbContext>(setup =>
                {
                    setup.UseSqlServer(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(typeof(Program).Assembly.FullName);
                    });
                });
        }
    }
}
