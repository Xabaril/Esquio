using Esquio.Abstractions;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Esquio.Http.Store
{
    public class ServiceCollectionExtensionsTest
    {
        public ConfigurationBuilder ConfigurationBuilder { get; }
        public IWebHostEnvironment Environment { get; }
        public ServiceCollection ServiceCollection { get; }

        public ServiceCollectionExtensionsTest()
        {
            ConfigurationBuilder = new ConfigurationBuilder();
            ServiceCollection = new ServiceCollection();
            // Kinda hacky, but not sure how else to mock out the env...
            Environment = new HostBuilder()
                .ConfigureWebHost(h => h.UseEnvironment("Development"))
                .Build()
                .Services.GetService<IWebHostEnvironment>();
        }
        [Fact]
        public void include_mandatory_services_in_container()
        {
            ServiceCollection
                .AddLogging()
                .AddEsquio()
                .AddAspNetCoreDefaultServices()
                .AddHttpStore(setup => setup.UseBaseAddress("https://baseAddress"));

            var sp = ServiceCollection.BuildServiceProvider();
            var fs =sp.GetService<IFeatureService>();
        }
    }
}