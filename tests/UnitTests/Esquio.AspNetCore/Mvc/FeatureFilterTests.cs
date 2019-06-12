using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Mvc
{
    [Collection(nameof(AspNetCoreServer))]
    public class feature_filter_should
    {
        private readonly ServerFixture _fixture;
        public feature_filter_should(ServerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task excute_action_if_feature_is_enabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithEnabledFlag");

            response.StatusCode
                .Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task excute_action_if_all_features_are_enabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithMultipleEnabledFlag");

            response.StatusCode
                .Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task redirect_to_not_found_if_one_feature_is_disabled_when_use_multiple_features()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithMultipleFlagAndDisabled");

            response.StatusCode
                .Should().Be(StatusCodes.Status302Found);
        }

        [Fact]
        public async Task redirect_to_not_found_if_feature_is_disabled_and_no_fallback_action_is_configured()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithDisabledFlag");

            response.StatusCode
                .Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task redirect_to_configured_fallback_if_feature_is_disabled()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithDisabledFlagAndFallbackAction");

            response.StatusCode
                .Should().Be(StatusCodes.Status302Found);
        }
    }
}
