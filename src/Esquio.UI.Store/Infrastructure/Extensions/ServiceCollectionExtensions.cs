using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Store.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure<StoreOptions>(setup);

            return services.AddEntityFramework(configuration);
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<StoreDbContext, StoreDbContext>(setup => setup.SetupDbStoreFromEnvironment(configuration));
        }
        public static DbContextOptionsBuilder SetupDbStoreFromEnvironment(this DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration){
            if (Convert.ToBoolean(configuration["Store:UseSqlServer"]))
            {
                var connectionString = configuration.GetConnectionString("Esquio");
                contextOptionsBuilder.UseSqlServer(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                    });
                return contextOptionsBuilder;
            }
            else if (Convert.ToBoolean(configuration["Store:UseNpgSql"]))
            {
                var connectionString = configuration.GetConnectionString("EsquioNpgSql");
                contextOptionsBuilder.UseNpgsql(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                    });
                return contextOptionsBuilder;
            }

            throw new InvalidOperationException("Add EntityFramework requires either Store:UseSqlServer or Store:UseNpgsql to be set");
        }
    }
}
