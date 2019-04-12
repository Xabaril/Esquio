using Esquio.Configuration.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.Configuration.Store
{
    public class ConfigurationFeatureStoreTests
    {
        [Fact]
        public async Task Test()
        {
            var libraries = DependencyContext
                .Default
                .CompileLibraries
                .Where(c => !c.Name.Contains("System."))
                .Select(c => c.Name);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Esquio.Configuration.Store"))
                .AddJsonFile("appsettings.json")
                .Build();
            var store = new ConfigurationFeatureStore(
                configuration.GetSection("Esquio"),
                new LoggerFactory().CreateLogger< ConfigurationFeatureStore>(),
                libraries);
            var toggles = await store.FindTogglesTypesAsync("Mediaset", "FeatureA");
            var count = toggles.Count();
        }
    }
}
