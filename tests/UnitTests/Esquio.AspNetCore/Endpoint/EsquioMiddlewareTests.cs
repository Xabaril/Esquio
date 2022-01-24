using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Endpoint
{
    [Collection(nameof(AspNetCoreServer))]
    public class esquio_middleware_should
    {
        private readonly ServerFixture _fixture;
        public esquio_middleware_should(ServerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task response_feature_status_and_use_default_product()
        {
            var featureName = "Sample1";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName}")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(1);

            content.First()
                .Enabled
                .Should()
                .BeTrue();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName);
        }

        [Fact]
        public async Task response_feature_status_and_use_specified_product()
        {
            var featureName = "Sample1";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName}&productName=default")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(1);

            content.First()
                .Enabled
                .Should()
                .BeTrue();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName);
        }

        [Fact]
        public async Task response_multiple_feature_status_and_use_default_product()
        {
            var featureName1 = "Sample1";
            var featureName2 = "Sample2";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName1}&featureName={featureName2}")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(2);

            content.First()
                .Enabled
                .Should()
                .BeTrue();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName1);

            content.Last()
               .Enabled
               .Should()
               .BeFalse();

            content.Last()
                .Name
                .Should()
                .BeEquivalentTo(featureName2);
        }

        [Fact]
        public async Task response_multiple_feature_status_and_use_specified_product()
        {
            var featureName1 = "Sample1";
            var featureName2 = "Sample2";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName1}&featureName={featureName2}&productName=default")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(2);

            content.First()
                .Enabled
                .Should()
                .BeTrue();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName1);

            content.Last()
               .Enabled
               .Should()
               .BeFalse();

            content.Last()
                .Name
                .Should()
                .BeEquivalentTo(featureName2);
        }

        [Fact]
        public async Task response_feature_status_and_use_only_latest_product()
        {
            var featureName = "Sample1";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName}&productName=ProductA&productName=default")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(1);

            content.First()
                .Enabled
                .Should()
                .BeTrue();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName);
        }

        [Fact]
        public async Task response_disabled_if_feature_is_not_exist()
        {
            var featureName = "NonExisting";

            var response = await _fixture.TestServer
                .CreateRequest($"esquio?featureName={featureName}")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(1);

            content.First()
                .Enabled
                .Should()
                .BeFalse();

            content.First()
                .Name
                .Should()
                .BeEquivalentTo(featureName);
        }

        [Fact]
        public async Task response_empty_list_if_feature_is_not_specified()
        {
            var response = await _fixture.TestServer
                .CreateRequest($"esquio")
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<IEnumerable<EsquioResponse>>();

            content.Count()
                .Should().Be(0);
        }

        private class EsquioResponse
        {
            public bool Enabled { get; set; }

            public string Name { get; set; }
        }
    }
}
