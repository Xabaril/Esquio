using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.EntityFramework.Store
{
    [Collection(nameof(CollectionExecutionFixture))]
    public class EntityFrameworkCoreStoreTests
    {
        private readonly Fixture _fixture;

        public EntityFrameworkCoreStoreTests(Fixture fixture)
        {
            _fixture = fixture;
            _fixture.Options = Data.SelectMany(d => d.Select(o => (DbContextOptions<StoreDbContext>)o)).ToList();

            foreach (var options in _fixture.Options)
            {
                using (var context = new StoreDbContext(options, new StoreOptions()))
                {
                    context.Database.EnsureCreated();
                }
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task return_true_if_store_persisted_the_feature_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);

            var result = await store.AddFeatureAsync("testFeature", "testApplication", true);

            result.Should().Be(true);
        }

        public static TheoryData<DbContextOptions<StoreDbContext>> Data =>
            new TheoryData<DbContextOptions<StoreDbContext>>
            {
                //DatabaseProviderBuilder.BuildMySql<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests)),
                DatabaseProviderBuilder.BuildPostgreSql<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests)),
                DatabaseProviderBuilder.BuildPostgreSql<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests)),
                DatabaseProviderBuilder.BuildSqlServer<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests)),
                DatabaseProviderBuilder.BuildLocalDb<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests)),
                DatabaseProviderBuilder.BuildInMemory<StoreDbContext>(nameof(EntityFrameworkCoreStoreTests))
            };
    }

    class EntityFrameworkCoreStoreBuilder
    {
        public EntityFrameworkCoreStoreBuilder WithExistingData()
        {
            return this;
        }

        public EntityFrameworkCoreFeaturesStore Build(DbContextOptions<StoreDbContext> options)
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<EntityFrameworkCoreFeaturesStore>();
            var dbContext = new StoreDbContext(options, new StoreOptions());
            var store = new EntityFrameworkCoreFeaturesStore(logger, dbContext);

            return store;
        }
    }
}
