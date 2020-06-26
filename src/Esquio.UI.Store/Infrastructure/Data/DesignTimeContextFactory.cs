using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Esquio.UI.Store.Infrastructure.Data.DbContexts;

namespace Esquio.UI.Store.Infrastructure.Data
{
    public class DesignTimeContextFactory
        : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json");
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
            optionsBuilder.SetupDbStoreFromEnvironment(configBuilder.Build());

            var dbContextOptions = optionsBuilder.Options;
            var storeOptions = Options.Create(new StoreOptions());

            return new StoreDbContext(dbContextOptions, storeOptions);
        }
    }
    public class NpgSqlDesignTimeContextFactory
        : IDesignTimeDbContextFactory<NpgSqlContext>
    {
        public NpgSqlContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json");
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
            optionsBuilder.SetupDbStoreFromEnvironment(configBuilder.Build());

            var dbContextOptions = optionsBuilder.Options;
            var storeOptions = Options.Create(new StoreOptions());

            return new NpgSqlContext(dbContextOptions, storeOptions);
        }
    }
}
