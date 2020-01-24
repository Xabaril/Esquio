using Esquio;
using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Diagnostics;
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
                //add product
                var product = new ProductEntity("default", "description");
                dbContext.Products.Add(product);
                dbContext.SaveChanges();

                //add ring

                var defaultRing = new RingEntity(product.Id, "Test", byDefault: true);
                dbContext.Rings.Add(defaultRing);

                var productionRing = new RingEntity(product.Id, "Production", byDefault: false);
                dbContext.Rings.Add(productionRing);

                dbContext.SaveChanges();

                //add default default with toggle on default ring
                var feature = new FeatureEntity(product.Id, "app-feature", enabled: true, archived: false);
                var toggle = new ToggleEntity(feature.Id, "Esquio.Toggles.XToggle");

                var stringparameterOnDefaultRing = new ParameterEntity(toggle.Id, defaultRing.Name, "strparam", "value1");
                toggle.Parameters.Add(stringparameterOnDefaultRing);

                var stringparameterOnProductionRing = new ParameterEntity(toggle.Id, productionRing.Name, "strparam", "value2");
                toggle.Parameters.Add(stringparameterOnProductionRing);

                var intparameterOnDefaultRing = new ParameterEntity(toggle.Id, defaultRing.Name, "intparam", "1");
                toggle.Parameters.Add(intparameterOnDefaultRing);

                var intparameterOnProductionRing = new ParameterEntity(toggle.Id, productionRing.Name, "intparam", "2");
                toggle.Parameters.Add(intparameterOnProductionRing);

                feature.Toggles
                    .Add(toggle);

                dbContext.Features.Add(feature);

                dbContext.SaveChanges();
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_null_when_product_not_exist(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("non-existing-feature", "non-existing-product", EsquioConstants.DEFAULT_RING_NAME);

            expected.Should()
                .BeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_null_when_feature_is_not_configured_on_product(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("non-existing-feature", EsquioConstants.DEFAULT_PRODUCT_NAME, EsquioConstants.DEFAULT_RING_NAME);

            expected.Should()
                .BeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_null_when_feature_exist_on_diferent_product(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("app-feature", "non-existing-product", EsquioConstants.DEFAULT_RING_NAME);

            expected.Should()
                .BeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_back_feature_when_is_configured(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store.FindFeatureAsync("app-feature", EsquioConstants.DEFAULT_PRODUCT_NAME, EsquioConstants.DEFAULT_RING_NAME);

            expected.Should()
                .NotBeNull();

            expected.GetToggles()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .Count().Should().Be(2);

            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Name.Should().Be("strparam");
            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Value.Should().Be("value1");
            expected.GetToggles()
               .First()
               .GetParameters()
               .Last()
               .Name.Should().Be("intparam");

            expected.GetToggles()
                .First()
                .GetParameters()
                .Last()
                .Value.Should().Be("1");
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task get_back_feature_with_specified_ring_parameters_when_is_configured(DbContextOptions<StoreDbContext> options)
        {
            var store = new EntityFrameworkCoreStoreBuilder(options)
                .Build();

            var expected = await store
                .FindFeatureAsync("app-feature", EsquioConstants.DEFAULT_PRODUCT_NAME, "Production");

            expected.Should()
                .NotBeNull();

            expected.GetToggles()
                .Count().Should().Be(1);

            expected.GetToggles()
                .First()
                .GetParameters()
                .Count().Should().Be(2);

            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Name.Should().Be("strparam");

            expected.GetToggles()
                .First()
                .GetParameters()
                .First()
                .Value.Should().Be("value2");

            expected.GetToggles()
                .First()
                .GetParameters()
                .Last()
                .Name.Should().Be("intparam");

            expected.GetToggles()
                .First()
                .GetParameters()
                .Last()
                .Value.Should().Be("2");
        }

        public static TheoryData<DbContextOptions<StoreDbContext>> Data =>
            new TheoryData<DbContextOptions<StoreDbContext>>
            {
                DatabaseProviderBuilder.BuildSqlServer<StoreDbContext>(nameof(entityframeworkcore_store_should)),
            };
    }

    class EntityFrameworkCoreStoreBuilder
    {
        private readonly DbContextOptions<StoreDbContext> _options;

        public EntityFrameworkCoreStoreBuilder(DbContextOptions<StoreDbContext> options)
        {
            _options = options;
        }

        public EntityFrameworkCoreFeaturesStore Build()
        {
            var loggerFactory = new LoggerFactory();
            var diagnostics = new EsquioEntityFrameworkCoreStoreDiagnostics(loggerFactory);

            var dbContext = new StoreDbContext(_options, new StoreOptions());
            var store = new EntityFrameworkCoreFeaturesStore(dbContext, diagnostics);

            return store;
        }
    }
}
