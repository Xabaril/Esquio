using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Store.Infrastructure.Data;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;
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
            if (Convert.ToBoolean(configuration["Store:UseSqlServer"]))
            {
                return services.AddEntityFrameworkSqlServer(configuration);
            }
            else if (Convert.ToBoolean(configuration["Store:UseNpgSql"]))
            {
                return services.AddEntityFrameworkNpgSql(configuration);
            }

            throw new InvalidOperationException("Add EntityFramework requires either Store:UseSqlServer or Store:UseNpgsql to be set");
        }
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Esquio");
            return services.AddDbContext<StoreDbContext>(builder => builder.SetupSqlServer(connectionString));
        }
        public static IServiceCollection AddEntityFrameworkNpgSql(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EsquioNpgSql");
            return services.AddDbContext<StoreDbContext, NpgSqlContext>(builder => builder.SetupNpgSql(connectionString));
        }
        public static DbContextOptionsBuilder SetupSqlServer(this DbContextOptionsBuilder contextOptionsBuilder, string connectionString)
        {
            return contextOptionsBuilder.UseSqlServer(connectionString, setup =>
                {
                    setup.MaxBatchSize(10);
                    setup.EnableRetryOnFailure();

                    setup.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                });
        }
        public static DbContextOptionsBuilder SetupNpgSql(this DbContextOptionsBuilder contextOptionsBuilder, string connectionString)
        {
            return contextOptionsBuilder.UseNpgsql(connectionString, setup =>
                    {
                        setup.MaxBatchSize(10);
                        setup.EnableRetryOnFailure();

                        setup.MigrationsAssembly(typeof(NpgSqlDesignTimeContextFactory).Assembly.FullName);
                    });
        }
    }
}
