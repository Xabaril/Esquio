using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Linq;

namespace Esquio.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigrateDbContext<StoreDbContext>((context,sp)=>
                {
                    if ( !context.Permissions.Any())
                    {
                        context.Permissions.Add(new PermissionEntity()
                        {
                            SubjectId = "1",
                            ReadPermission = true,
                            WritePermission = true,
                            ManagementPermission = true,
                        });

                        context.SaveChanges();
                    }
                })
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
