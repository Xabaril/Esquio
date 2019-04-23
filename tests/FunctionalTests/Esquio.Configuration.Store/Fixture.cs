using Esquio.Abstractions;
using Esquio.Configuration.Store;
using Esquio.Configuration.Store.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace FunctionalTests.Esquio.Configuration.Store
{
    public class Fixture
    {
        public IFeatureStore FeatureStore { get; }

        public Fixture()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Esquio.Configuration.Store"))
               .AddJsonFile("appsettings.json")
               .Build();

            var esquio = new EsquioConfiguration();
            configuration.Bind("Esquio", esquio);

            var options = Options.Create(esquio);
            var logger = new LoggerFactory().CreateLogger<ConfigurationFeatureStore>();

            FeatureStore = new ConfigurationFeatureStore(options, logger);
        }
    }
}
