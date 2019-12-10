using Esquio.UI.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Esquio.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
                .MigrateDbContext(StoreDbContextSeed.Seed())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
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
            });

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((hostingContext, configuration) =>
        //        {
        //            Log.Logger = new LoggerConfiguration()
        //                .ReadFrom.Configuration(configuration.Build())
        //                .CreateLogger();
        //        })
        //        .ConfigureLogging((hostingContext, logging) =>
        //        {
        //            logging.ClearProviders();
        //            logging.AddSerilog(dispose: true);
        //        })
        //        .UseStartup<Startup>();
    }
}
