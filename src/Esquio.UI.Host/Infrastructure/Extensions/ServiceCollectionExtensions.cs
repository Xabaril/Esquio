using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
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
            services.Configure(setup);

            return AddEntityFramework(services, configuration);
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (Convert.ToBoolean(configuration["Store:UseSqlServer"]))
            {
                return services.AddEntityFrameworkSqlServer(configuration.GetConnectionString("Esquio"));
            }
            else if (Convert.ToBoolean(configuration["Store:UseNpgSql"]))
            {
                return services.AddEntityFrameworkNpgsql(configuration.GetConnectionString("EsquioNpgSql"));
            }

            throw new InvalidOperationException("Add EntityFramework requires either Store:UseSqlServer or Store:UseNpgsql to be set");
        }
    }
}
