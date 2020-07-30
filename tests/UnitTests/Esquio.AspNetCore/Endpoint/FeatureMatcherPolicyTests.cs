using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Endpoint
{
    [Collection(nameof(AspNetCoreServer))]
    public class featurematcherpolicy_should
    {
        private readonly ServerFixture _fixture;

        public featurematcherpolicy_should(ServerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task use_endpoint_with_metadata_when_feature_is_enabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/MultipleEndpointsWithFeatureSample1");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Enabled");
        }

        [Fact]
        public async Task use_alternative_endpoint_when_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/MultipleEndpointsWithFeatureSample2");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Disabled");
        }

        [Fact]
        public async Task use_configured_fallback_endpoint_when_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithFallback");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Endpoint fallback is executed!");
        }

        [Fact]
        public async Task use_default_configured_fallback_endpoint_when_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithNotFoundFallback");

            response.StatusCode
                .Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task use_endpoint_when_single_feature_is_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithFeatureActive");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Enabled");
        }

        [Fact]
        public async Task use_endpoint_when_multiple_feature_are_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithMultipleFeatureActive");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Enabled");
        }

        [Fact]
        public async Task get_404_when_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithFeatureDisabled");

            response.StatusCode
                .Should().Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task get_404_when_one_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/scenarios/SingleEndPointWithOneFeatureDisabled");

            response.StatusCode
                .Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
