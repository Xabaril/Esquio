using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Esquio.UI.Store.Infrastructure.Data.DbContexts
{
    public class NpgSqlContext : StoreDbContext
    {
        public NpgSqlContext(DbContextOptions dbContextOptions, IOptions<StoreOptions> storeOptions) : base(dbContextOptions, storeOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json");
            var connectionString = configBuilder.Build().GetConnectionString("EsquioNpgSql");
            optionsBuilder.SetupNpgSql(connectionString);
        }
    }
}