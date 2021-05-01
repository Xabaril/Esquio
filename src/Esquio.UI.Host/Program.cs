using Esquio.UI.Host.Infrastructure.Data.Seed;
using Esquio.UI.Host.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
                         .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                         {
                             var configuration = configurationBuilder.Build();
                             var appConfigurationOptions = configuration.GetAzureAppConfigurationOptions();

                             if (appConfigurationOptions.Enabled)
                             {
                                 configurationBuilder.AddAzureAppConfiguration(appConfigurationOptions);
                                 configuration = configurationBuilder.Build();
                             }

                             Log.Logger = new LoggerConfiguration()
                                 .ReadFrom.Configuration(configuration)
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
