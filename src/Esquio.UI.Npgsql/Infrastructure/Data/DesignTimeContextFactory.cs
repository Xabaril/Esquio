using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Esquio.UI.Npgsql.Data
{
    public class DesignTimeContextFactory
        : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();

            optionsBuilder.UseNpgsql("Server=tcp:localhost,5432;Database=Esquio.UI.Tests;User Id=postgres;Password=postgres",
                options => options.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName));

            var dbContextOptions = optionsBuilder.Options;
            var storeOptions = Options.Create(new StoreOptions());

            return new StoreDbContext(dbContextOptions, storeOptions);
        }
    }
}
