using Esquio;
using Esquio.Http.Store;
using Esquio.Http.Store.DependencyInjection;
using Esquio.Http.Store.Diagnostics;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.Http.Store
{
    [Collection(nameof(AspNetCoreServer))]
    public class esquio_http_store_should
    {
        private readonly ServerFixture _fixture;

        public esquio_http_store_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task get_null_if_features_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Ring()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Rings
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
                .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var apiKeyValue = "barkey";

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey(apiKeyValue)
                .WithValidTo(DateTime.UtcNow.AddDays(1))
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var store = CreateStore(apiKey: apiKey.Key);

            var featureModel = await store
                .FindFeatureAsync("non-existing", product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.Should()
                .BeNull();
        }

        [Fact]
        [ResetDatabase]
        public async Task get_feature_from_the_store()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Ring()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Rings
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
                .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var apiKeyValue = "barkey";

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey(apiKeyValue)
                .WithValidTo(DateTime.UtcNow.AddDays(1))
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var store = CreateStore(apiKey: apiKey.Key);

            var featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.Name
                .Should()
                .BeEquivalentTo(feature.Name);

            featureModel.IsEnabled
                .Should()
                .Be(true);

            featureModel.GetToggles()
                .Count()
                .Should().Be(1);

            featureModel.GetToggles()
                .First()
                .Type
                .Should()
                .BeEquivalentTo("Esquio.Toggles.EnvironmentToggle,Esquio");

            featureModel.GetToggles()
                .First()
                .GetParameters()
                .Count()
                .Should()
                .Be(1);

            featureModel.GetToggles()
               .First()
               .GetParameters()
               .First()
               .Name
               .Should()
               .BeEquivalentTo("Environments");

            featureModel.GetToggles()
               .First()
               .GetParameters()
               .First()
               .Value
               .Should()
               .BeEquivalentTo("Development");
        }

        [Fact]
        [ResetDatabase]
        public async Task get_feature_from_the_store_use_cache_if_configured()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Ring()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Rings
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
                .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var apiKeyValue = "barkey";

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey(apiKeyValue)
                .WithValidTo(DateTime.UtcNow.AddDays(1))
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var cache = CreateDefaultCache();
            var store = CreateCachedStore(cache, apiKey: apiKey.Key, useCache: true);

            var featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.Name
                .Should()
                .BeEquivalentTo(feature.Name);

            var cacheKey = CacheKeyCreator.GetCacheKey(product.Name, feature.Name, defaultRing.Name,"3.0");

            var entry = await cache.GetStringAsync(cacheKey);

            entry.Should()
                .NotBeNullOrWhiteSpace();

            await cache.SetStringAsync(cacheKey, "{\"featureName\":\"barfeature\",\"enabled\":false}");

            featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.IsEnabled
                .Should().BeFalse();
        }

        [Fact]
        [ResetDatabase]
        public async Task get_feature_from_the_store_throw_if_cache_is_enabled_but_distributed_cache_service_is_null()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Ring()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Rings
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
                .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var apiKeyValue = "barkey";

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey(apiKeyValue)
                .WithValidTo(DateTime.UtcNow.AddDays(1))
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var store = CreateCachedStore(null, apiKey: apiKey.Key, useCache: true);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await store.FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);
            });
        }

        [Fact]
        [ResetDatabase]
        public async Task get_feature_from_the_store_dont_use_cache_if_isnot_configured()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Ring()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Rings
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
                .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var apiKeyValue = "barkey";

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey(apiKeyValue)
                .WithValidTo(DateTime.UtcNow.AddDays(1))
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var cache = CreateDefaultCache();
            var store = CreateCachedStore(cache, apiKey: apiKey.Key, useCache: false);

            var featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.Name
                .Should()
                .BeEquivalentTo(feature.Name);

            var cacheKey = CacheKeyCreator.GetCacheKey(product.Name, feature.Name, defaultRing.Name,"3.0");

            var entry = await cache.GetStringAsync(cacheKey);

            entry.Should()
                .BeNull();

            await cache.SetStringAsync(cacheKey, "{\"featureName\":\"barfeature\",\"enabled\":false}");

            featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            featureModel.IsEnabled
                .Should().BeTrue();
        }


        private EsquioHttpStore CreateStore(string apiKey)
        {
            var storeOptions = Options.Create(new HttpStoreOptions()
            {
                CacheEnabled = false,
                ApiKey = apiKey,
                BaseAddress = new Uri("http://localhost")
            });

            var diagnostics = new EsquioHttpStoreDiagnostics(new LoggerFactory());

            return new EsquioHttpStore(
                new TestServerHttpClientFactory(_fixture, apiKey),
                storeOptions,
                diagnostics);
        }

        private EsquioHttpStore CreateCachedStore(IDistributedCache cache, string apiKey, bool useCache = true)
        {
            var storeOptions = Options.Create(new HttpStoreOptions()
            {
                CacheEnabled = useCache,
                ApiKey = apiKey,
                BaseAddress = new Uri("http://localhost")
            });

            var diagnostics = new EsquioHttpStoreDiagnostics(new LoggerFactory());

            return new EsquioHttpStore(
                new TestServerHttpClientFactory(_fixture, apiKey),
                storeOptions,
                diagnostics,
                cache);
        }

        private IDistributedCache CreateDefaultCache()
        {
            var cacheOptions = Options.Create(
              new MemoryDistributedCacheOptions());

            return new MemoryDistributedCache(cacheOptions);
        }
    }

    class TestServerHttpClientFactory
        : IHttpClientFactory
    {
        private readonly ServerFixture _fixture;
        private readonly string _apiKey;

        public TestServerHttpClientFactory(ServerFixture fixture, string apiKey)
        {
            _fixture = fixture;
            _apiKey = apiKey;
        }
        public HttpClient CreateClient(string name)
        {
            var httpClient = _fixture.TestServer
                .CreateClient();

            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);

            return httpClient;
        }
    }
}
