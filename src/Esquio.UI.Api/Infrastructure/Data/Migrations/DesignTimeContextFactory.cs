using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Esquio.UI.Store.Infrastructure.Data
{
    public class DesignTimeContextFactory
        : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();

            //using default docker-compose connection string, only need for creating migrations on this project
            optionsBuilder.SetupSqlServer("Server=tcp:localhost,5433;Initial Catalog=Esquio.UI;User Id=sa;Password=Password12");

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
            var optionsBuilder = new DbContextOptionsBuilder<NpgSqlContext>();

            //using default docker-compose connection string, only need for creating migrations on this project
            optionsBuilder.SetupNpgSql("Host=localhost;Port=5434;Database=Esquio.UI.Tests;User Id=postgres;Password=Password12!");

            var dbContextOptions = optionsBuilder.Options;

            // Public is the default schema for Postgres
            var storeOptions = Options.Create(new StoreOptions { DefaultSchema = "public" });

            return new NpgSqlContext(dbContextOptions, storeOptions);
        }
    }

    public class MySqlDesignTimeContextFactory
       : IDesignTimeDbContextFactory<MySqlContext>
    {
        public MySqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();

            //using default docker-compose connection string, only need for creating migrations on this project
            optionsBuilder.SetupMySql("Server=localhost;Database=Esquio.UI.Tests;Uid=mysql;Pwd=Password12!;");

            var dbContextOptions = optionsBuilder.Options;

            // Public is the default schema for Postgres
            var storeOptions = Options.Create(new StoreOptions { DefaultSchema = "" });

            return new MySqlContext(dbContextOptions, storeOptions);
        }
    }
}
