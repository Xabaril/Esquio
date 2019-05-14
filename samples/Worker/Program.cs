using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseDefaultServiceProvider(options=>
                //{
                //    options.ValidateScopes = true;
                //})
                .ConfigureServices((context, services) =>
                {
                    services.AddEsquio()
                        .AddConfigurationStore(context.Configuration, key: "Esquio")
                    .Services
                    .AddHostedService<Worker>();
                });
    }
}
