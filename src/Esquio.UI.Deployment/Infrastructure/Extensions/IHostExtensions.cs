using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    static class IHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder = null)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TContext>();

                try
                {
                    context.Database.Migrate();

                    seeder?.Invoke(context, scope.ServiceProvider);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

                    logger.LogError(ex, $"An error occurred while migrating the database for context {nameof(TContext)}.");
                }
            }

            return host;
        }
    }
}
