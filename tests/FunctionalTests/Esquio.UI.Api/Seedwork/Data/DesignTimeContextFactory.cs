using Esquio.EntityFrameworkCore.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}
