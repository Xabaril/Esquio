using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;

namespace UnitTests.Esquio.AspNetCore.Extensions
{
    public class addsquio_should
    {
        [Fact]
        public void include_mandatory_services_in_container()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddEsquio()
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(configurationBuilder.Build());

            var serviceProvider = serviceCollection.BuildServiceProvider();

        }
        [Fact(DisplayName ="Add EntityFramework should throw if no store setup to be 'used'" )]
        public void AddEntityFramework001(){
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();
            var exception = Assert.Throws<InvalidOperationException>(() => serviceCollection.AddEntityFramework(configuration));
            Assert.Equal("Add EntityFramework requires either Store:UseSqlServer or Store:UseNpgsql to be set", exception.Message);
        }
        [Fact(DisplayName ="Add EntityFramework should use SqlServer if UseSqlServer set in configuration" )]
        public void AddEntityFramework002(){
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                    new Dictionary<string,string>{
                        {"Store:UseSqlServer", "true"},
                        {"ConnectionStrings:Esquio", "SomeConnectionString"}
                    }
            ).Build();
            serviceCollection.AddEntityFramework(configuration);
            _ = serviceCollection.BuildServiceProvider();
            var dbContext = serviceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(StoreDbContext), dbContext.ImplementationType);
        }
        [Fact(DisplayName ="Add EntityFramework should use NpgSql if UseNpgSql set in configuration" )]
        public void AddEntityFramework003(){
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                    new Dictionary<string,string>{
                        {"Store:UseNpgSql", "true"},
                        {"ConnectionStrings:EsquioNpgSql", "SomePostgresConnectionString"}
                    }
            ).Build();
            serviceCollection.AddEntityFramework(configuration);
            _ = serviceCollection.BuildServiceProvider();
            var dbContext = serviceCollection.First(c => c.ServiceType == typeof(StoreDbContext));
            Assert.Equal(typeof(NpgSqlContext), dbContext.ImplementationType);
        }
    }
}
