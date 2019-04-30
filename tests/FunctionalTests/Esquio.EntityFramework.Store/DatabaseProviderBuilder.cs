using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    public class DatabaseProviderBuilder
    {
        static readonly Fixture ExecutionFixture = new Fixture();

        public static DbContextOptions<T> BuildInMemory<T>(string name) where T : DbContext
        {
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseInMemoryDatabase(name);
            return builder.Options;
        }
        public static DbContextOptions<T> BuildSqlite<T>(string name) where T : DbContext
        {
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseSqlite($"Filename=./Test.Esquio.EntityFramework-3.0.0.{name}.db");
            return builder.Options;
        }
        public static DbContextOptions<T> BuildLocalDb<T>(string name) where T : DbContext
        {
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseSqlServer(
                $@"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.Esquio.EntityFramework-3.0.0.{name};trusted_connection=yes;");
            return builder.Options;
        }
        public static DbContextOptions<T> BuildSqlServer<T>(string name) where T : DbContext
        {
            var connectionString = ExecutionFixture.IsAppVeyorExecution ?
                $@"Server=(local)\SQL2016;Database=Test.Esquio.EntityFramework-3.0.0.{name};User ID=sa;Password=Password12!" :
                $@"Server=tcp:localhost,1833;Database=Test.Esquio.EntityFramework-3.0.0.{name};User ID=sa;Password=Password12!";
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseSqlServer(connectionString);
            return builder.Options;
        }
        public static DbContextOptions<T> BuildPostgreSql<T>(string name) where T : DbContext
        {
            var connectionString = ExecutionFixture.IsAppVeyorExecution ?
                $@"Host=localhost;Database=Test.Esquio-3.0.0.{name};Username=postgres;Password=Password12!;Port=5432;" :
                $@"Host=localhost;Database=Test.Esquio-3.0.0.{name};Username=postgres;Password=Password12!;Port=5432;";
           var builder = new DbContextOptionsBuilder<T>();
            builder.UseNpgsql(connectionString);
            return builder.Options;
        }
    }
}
