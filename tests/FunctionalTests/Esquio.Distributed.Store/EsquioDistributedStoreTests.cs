using Esquio;
using Esquio.Distributed.Store;
using Esquio.Distributed.Store.Diagnostics;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.Distributed.Store
{
    [Collection(nameof(AspNetCoreServer))]
    public class esquio_distributed_store_should
    {
        private readonly ServerFixture _fixture;

        public esquio_distributed_store_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task get_feature_from_the_store()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
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
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var store = new EsquioDistributedStore(
                new TestServerHttpClientFactory(_fixture, apiKeyValue),
                new EsquioDistributedStoreDiagnostics(new LoggerFactory()));

            var featureModel = await store
                .FindFeatureAsync(feature.Name, product.Name, EsquioConstants.DEFAULT_RING_NAME);

            //TODO: add assertions
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
