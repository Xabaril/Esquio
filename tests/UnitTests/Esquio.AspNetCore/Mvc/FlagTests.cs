using FluentAssertions;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Mvc
{
    [Collection(nameof(AspNetCoreServer))]
    public class FlagConstrainShould
    {
        ServerFixture _fixture;

        public FlagConstrainShould(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task enable_action_when_flag_is_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithFlag");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("Active");
        }

        [Fact]
        public async Task use_default_action_when_flag_is_not_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithFlagNoActive");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("NonActive");
        }
    }
}
