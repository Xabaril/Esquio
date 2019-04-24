using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Mvc
{
    [Collection(nameof(AspNetCoreServer))]
    public class FlagShould
    {
        private readonly ServerFixture _fixture;
        public FlagShould(ServerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }
        [Fact]
        public async Task excute_action_if_feature_is_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithActiveFlag");

            response.StatusCode
                .Should().Be(StatusCodes.Status200OK);
        }
        [Fact]
        public async Task redirect_to_not_found_if_feature_is_not_active_and_no_fallback_action_is_configured()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithNoActiveFlag");

            response.StatusCode
                .Should().Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task redirect_to_configured_fallback_if_feature_is_not_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithNoActiveFlagAndFallbackAction");

            response.StatusCode
                .Should().Be(StatusCodes.Status302Found);
        }
    }
}
