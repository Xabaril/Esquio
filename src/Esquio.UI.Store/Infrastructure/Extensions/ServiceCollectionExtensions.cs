using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Store.Infrastructure.Data;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure<StoreOptions>(setup);

            return services.AddEntityFramework(configuration, environment);
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            switch (configuration["Data:Store"])
            {
                case "NpgSql":
                    {
                        return services.AddEntityFrameworkNpgSql(configuration, environment);
                    }
                case "SqlServer":
                    {
                        return services.AddEntityFrameworkSqlServer(configuration, environment);
                    }

                default: throw new InvalidOperationException("Add EntityFramework requires either Data:Store:SqlServer or Data:Store:Npgsql to be set.");
            }

        }
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration["Data:ConnectionString"];
            return services.AddDbContext<StoreDbContext>(builder => builder.SetupSqlServer(connectionString).SetupSensitiveLogging(environment));
        }
        public static IServiceCollection AddEntityFrameworkNpgSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration["Data:ConnectionString"];
            services.Configure<StoreOptions>(setup => setup.DefaultSchema = "public");
            return services.AddDbContext<StoreDbContext, NpgSqlContext>(builder => builder.SetupNpgSql(connectionString).SetupSensitiveLogging(environment));
        }
        public static DbContextOptionsBuilder SetupSensitiveLogging(this DbContextOptionsBuilder optionsBuilder, IWebHostEnvironment environment)
        {
            return optionsBuilder.EnableDetailedErrors()
                    .EnableSensitiveDataLogging(environment.IsDevelopment());
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
