using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;

namespace UnitTests.Esquio.AspNetCore.Extensions
{
    public class addsquio_should
    {
        public ConfigurationBuilder ConfigurationBuilder { get; }
        public IWebHostEnvironment Environment { get; }
        public ServiceCollection ServiceCollection { get; }

        public addsquio_should(){
            ConfigurationBuilder = new ConfigurationBuilder();
            ServiceCollection = new ServiceCollection();
            // Kinda hacky, but not sure how else to mock out the env...
            Environment = new HostBuilder().ConfigureWebHost(h => h.UseEnvironment("Development")).Build().Services.GetService<IWebHostEnvironment>();
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
        [Fact(DisplayName ="Add EntityFramework should throw if no store setup to be 'used'" )]
        public void AddEntityFramework001(){
            var exception = Assert.Throws<InvalidOperationException>(() => ServiceCollection.AddEntityFramework(ConfigurationBuilder.Build(), Environment));
            Assert.Equal("Add EntityFramework requires either Store:UseSqlServer or Store:UseNpgsql to be set", exception.Message);
        }
        [Fact(DisplayName ="Add EntityFramework should use SqlServer if UseSqlServer set in configuration" )]
        public void AddEntityFramework002(){
            var configuration = ConfigurationBuilder.AddInMemoryCollection(
                    new Dictionary<string,string>{
                        {"Store:UseSqlServer", "true"},
                        {"ConnectionStrings:Esquio", "SomeConnectionString"}
                    }
            ).Build();
            ServiceCollection.AddEntityFramework(configuration, Environment);
            _ = ServiceCollection.BuildServiceProvider();
            var dbContext = ServiceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(StoreDbContext), dbContext.ImplementationType);
        }
        [Fact(DisplayName ="Add EntityFramework should use NpgSql if UseNpgSql set in configuration" )]
        public void AddEntityFramework003(){
            var configuration = ConfigurationBuilder.AddInMemoryCollection(
                    new Dictionary<string,string>{
                        {"Store:UseNpgSql", "true"},
                        {"ConnectionStrings:EsquioNpgSql", "SomePostgresConnectionString"}
                    }
            ).Build();
            ServiceCollection.AddEntityFramework(configuration, Environment);
            _ = ServiceCollection.BuildServiceProvider();
            var dbContext = ServiceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(NpgSqlContext), dbContext.ImplementationType);
        }
    }
}
