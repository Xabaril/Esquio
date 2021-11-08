using Esquio.UI.Deployment.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Esquio.UI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices((context, services) =>
                        {
                            services.AddEsquioServices(context.Configuration, context.HostingEnvironment);
                        })
                        .Configure((context, app) =>
                        {
                            var apiVersion = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                            app.ConfigureEsquio(context.Configuration, context.HostingEnvironment, apiVersion);
                        });
                });
    }
}
