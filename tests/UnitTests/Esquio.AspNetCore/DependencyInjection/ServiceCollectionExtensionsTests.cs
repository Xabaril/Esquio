using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Extensions
{
    public class addsquio_should
    {
        public ConfigurationBuilder ConfigurationBuilder { get; }
        public IWebHostEnvironment Environment { get; }
        public ServiceCollection ServiceCollection { get; }

        public addsquio_should()
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
                .AddConfigurationStore(ConfigurationBuilder.Build());

            _ = ServiceCollection.BuildServiceProvider();

        }
        [Fact()]
        public void throw_when_no_store_is_specified()
        {
            Assert.Throws<InvalidOperationException>(() => ServiceCollection.AddEntityFramework(ConfigurationBuilder.Build(), Environment));
        }
        [Fact]
        public void use_sqlserver_when_is_configured()
        {
            var configuration = ConfigurationBuilder.AddInMemoryCollection(
                    new Dictionary<string, string>{
                        {"Data:Store", "SqlServer"},
                        {"Data:ConnectionString", "SomeConnectionString"}
                    }
            ).Build();

            ServiceCollection.AddEntityFramework(configuration, Environment);

            _ = ServiceCollection.BuildServiceProvider();

            var dbContext = ServiceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(StoreDbContext), dbContext.ImplementationType);
        }
        [Fact]
        public void use_npgsql_when_is_configured()
        {
            var configuration = ConfigurationBuilder.AddInMemoryCollection(
                    new Dictionary<string, string>{
                        {"Data:Store", "NpgSql"},
                        {"Data:ConnectionString", "SomePostgresConnectionString"}
                    }
            ).Build();
            ServiceCollection.AddEntityFramework(configuration, Environment);
            _ = ServiceCollection.BuildServiceProvider();
            var dbContext = ServiceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(NpgSqlContext), dbContext.ImplementationType);
        }
    }
}
