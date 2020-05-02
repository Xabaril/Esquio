using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Data
{
    public class DesignTimeContextFactory
        : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();

            optionsBuilder.UseSqlServer("Server=tcp:localhost,5433;Initial Catalog=Esquio.UI.Tests;User Id=sa;Password=Password12!", options =>
            {
                options.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
            });

            var dbContextOptions = optionsBuilder.Options;
            var storeOptions = Options.Create(new StoreOptions());

            return new StoreDbContext(dbContextOptions, storeOptions);
        }
    }
}
