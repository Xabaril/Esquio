using Esquio.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddEsquio()
                .AddConfigurationStore(configuration, "Esquio")
                .Services;

            var serviceProvider = services.BuildServiceProvider();

            var featureService = serviceProvider.GetService<IFeatureService>();
            if (await featureService.IsEnabledAsync("Colored"))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }

            Console.WriteLine("Welcome to Esquio!");
            Console.Read();
        }
    }
}
