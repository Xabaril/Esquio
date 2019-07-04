using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    public class DatabaseProviderBuilder
    {
        static readonly Fixture ExecutionFixture = new Fixture();

        public static DbContextOptions<T> BuildSqlServer<T>(string name) where T : DbContext
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();


            var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("Esquio"));
            connectionStringBuilder.InitialCatalog = $"Test.Esquio.EntityFramework-3.0.0.{name}";


            var builder = new DbContextOptionsBuilder<T>();
            builder.UseSqlServer(connectionStringBuilder.ConnectionString);
            return builder.Options;
        }
    }
}
