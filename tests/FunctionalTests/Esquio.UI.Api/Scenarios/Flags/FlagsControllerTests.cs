using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Flags
{
    [Collection(nameof(AspNetCoreServer))]
    public class flags_controller_should
    {
        private readonly ServerFixture _fixture;
        public flags_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        //[Fact] //Todo:IS NOT WORKING RIGH NOW
        public async Task response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Rollout(1, 1))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
    }
}
