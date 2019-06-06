using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
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
    public class entityframeworkcore_store_should
    {
        private const string Users = nameof(Users);
        private readonly Fixture _fixture;

        public entityframeworkcore_store_should(Fixture fixture)
        {
            _fixture = fixture;
            _fixture.Options = Data.SelectMany(d => d.Select(o => (DbContextOptions<StoreDbContext>)o)).ToList();

            foreach (var options in _fixture.Options)
            {
                using (var context = new StoreDbContext(options, new StoreOptions()))
                {
                    context.Database.EnsureCreated();

                    AddData(context);
                }
            }
        }

        void AddData(StoreDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var product = new ProductEntity("default", "description");
                dbContext.Products.Add(product);

                dbContext.SaveChanges();

                var appFeature = new FeatureEntity(product.Id, "app-feature", enabled: true);

                var toggle = new ToggleEntity(appFeature.Id, "Esquio.Toggles.XToggle");
                var parameter = new ParameterEntity(toggle.Id, "p1", "value1");
                toggle.Parameters.Add(parameter);
                appFeature.Toggles.Add(toggle);


                dbContext.Features.Add(appFeature);

                dbContext.SaveChanges();
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_null_when_product_not_exist(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("non-existing-feature", "non-existing-product");

            expected.Should()
                .BeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_null_when_feature_is_not_configured_on_product(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("non-existing-feature", "default");

            expected.Should()
                .BeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_back_feature_when_is_configured(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("app-feature", "default");

            expected.Should()
                .NotBeNull();

            expected.GetToggles()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Name.Should().Be("p1");
            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Value.Should().Be("value1");
        }
        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_back_feature_when_is_configured_and_default_product_is_used(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("app-feature");

            expected.Should()
                .NotBeNull();

            expected.GetToggles()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Name.Should().Be("p1");
            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Value.Should().Be("value1");
        }

        public static TheoryData<DbContextOptions<StoreDbContext>> Data =>
            new TheoryData<DbContextOptions<StoreDbContext>>
            {
                DatabaseProviderBuilder.BuildPostgreSql<StoreDbContext>(nameof(entityframeworkcore_store_should)),
                DatabaseProviderBuilder.BuildSqlServer<StoreDbContext>(nameof(entityframeworkcore_store_should)),
                DatabaseProviderBuilder.BuildLocalDb<StoreDbContext>(nameof(entityframeworkcore_store_should)),
                DatabaseProviderBuilder.BuildInMemory<StoreDbContext>(nameof(entityframeworkcore_store_should))
            };
    }

    class EntityFrameworkCoreStoreBuilder
    {
        private DbContextOptions<StoreDbContext> _options;

        public EntityFrameworkCoreStoreBuilder(DbContextOptions<StoreDbContext> options)
        {
            _options = options;
        }

        public EntityFrameworkCoreFeaturesStore Build()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<EntityFrameworkCoreFeaturesStore>();
            var dbContext = new StoreDbContext(_options, new StoreOptions());
            var store = new EntityFrameworkCoreFeaturesStore(logger, dbContext);

            return store;
        }
    }
}
