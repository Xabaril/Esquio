using Esquio.UI.Host.Infrastructure.Configuration;
using Esquio.UI.Host.Infrastructure.Data.Seed;
using Esquio.UI.Host.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Esquio.UI.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext(StoreDbContextSeed.Seed())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                         .ConfigureAppConfiguration((hostingContext, configuration) =>
                         {
                             if (AzureAppConfiguration.Enabled)
                             {
                                 configuration.UseAzureAppConfiguration();
                             }

                             Log.Logger = new LoggerConfiguration()
                                 .ReadFrom.Configuration(configuration.Build())
                                 .CreateLogger();
                         })
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders();
                            logging.AddSerilog(dispose: true);
                        });
                });
    }
}
