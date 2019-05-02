using FluentAssertions;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Mvc
{
    [Collection(nameof(AspNetCoreServer))]
    public class FeatureSwitchConstrainShould
    {
        ServerFixture _fixture;

        public FeatureSwitchConstrainShould(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task enable_action_when_flag_is_active()
        {
            var response = await _fixture.TestServer
                .CreateClient()
                .GetAsync("http://localhost/test/ActionWithFlagSwitch");

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
                .GetAsync("http://localhost/test/ActionWithFlagSwitchNoActive");

            response.IsSuccessStatusCode
                .Should().BeTrue();

            (await response.Content.ReadAsStringAsync())
                .Should().BeEquivalentTo("NonActive");
        }
    }
}
