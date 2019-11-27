using Esquio.UI.Infrastructure.Data.Seed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Esquio.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigrateDbContext(StoreDbContextSeed.Seed())
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration.Build())
                        .CreateLogger();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddSerilog(dispose: true);
                })
                .UseStartup<Startup>();
    }
}
