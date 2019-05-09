using Esquio;
using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Options;
using Esquio.Model;
using Esquio.Toggles;
using FluentAssertions;
using FunctionalTests.Base.Builders;
using FunctionalTests.Base.ObjectMother;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
                }
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void persisted_a_product_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);
            var product = ProductObjectMother.Create();

            Func<Task> expected = async () => await store.AddProductAsync(product);

            expected.Should().NotThrow();
        }
        [Theory]
        [MemberData(nameof(Data))]
        public async Task delete_a_product_when_it_does_exist_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);
            var product = ProductObjectMother.Create();

            await store.AddProductAsync(product);
            await store.DeleteProductAsync(product);
            Func<Task> expected = async () => await store.FindProductAsync(product.Name);

            expected.Should().Throw<EsquioException>();
        }


        [Theory]
        [MemberData(nameof(Data))]
        public async Task persisted_the_feature_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);
            var product = ProductObjectMother.Create();
            await store.AddProductAsync(product);

            Func<Task> expected = async () => await store.AddFeatureAsync(product.Name, FeatureObjectMother.Create());

            expected.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_back_a_feature_when_it_does_exist_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);
            var product = ProductObjectMother.Create();
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1;user2")
                .Build();
            var feature = Build
                .Feature($"{Constants.FeatureName}-{Guid.NewGuid()}")
                .AddOne(toggle)
                .Build();

            await store.AddProductAsync(product);
            await store.AddFeatureAsync(product.Name, feature);
            var expected = await store.FindFeatureAsync(feature.Name, product.Name);

            feature.Should().BeEquivalentTo(expected);
            feature.GetToggles().Count().Should().Be(expected.GetToggles().Count());
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task remove_a_feature_when_it_does_exist_on_database(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder().Build(options);
            var product = ProductObjectMother.Create();
            var feature = Build
                .Feature($"{Constants.FeatureName}-{Guid.NewGuid()}")
                .Build();

            await store.AddProductAsync(product);
            await store.AddFeatureAsync(product.Name, feature);
            var expected = await store.FindFeatureAsync(feature.Name, product.Name);

            feature.Should().BeEquivalentTo(expected);
            feature.GetToggles().Count().Should().Be(expected.GetToggles().Count());
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
