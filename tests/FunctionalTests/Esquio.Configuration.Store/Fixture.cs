using Esquio.Abstractions;
using Esquio.Configuration.Store;
using Esquio.Configuration.Store.Configuration;
using Esquio.Configuration.Store.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace FunctionalTests.Esquio.Configuration.Store
{
    public class Fixture
    {
        public IRuntimeFeatureStore FeatureStore { get; }

        public Fixture()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Esquio.Configuration.Store"))
               .AddJsonFile("appsettings.json")
               .Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<EsquioConfiguration>(configuration.GetSection("Esquio"))
                .BuildServiceProvider();

            var loggerFactory = new LoggerFactory();
            var diagnostics = new EsquioConfigurationStoreDiagnostics(loggerFactory);

            var options = serviceProvider.GetService<IOptionsSnapshot<EsquioConfiguration>>();
            FeatureStore = new ConfigurationFeatureStore(options, diagnostics);
        }
    }
}
