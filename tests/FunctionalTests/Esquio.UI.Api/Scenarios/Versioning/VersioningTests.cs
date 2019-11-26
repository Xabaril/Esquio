using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Versioning
{
    [Collection(nameof(AspNetCoreServer))]
    public class api_versioning_should
    {
        private readonly ServerFixture _fixture;
        public api_versioning_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task use_header_version_if_client_specify_booth()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.My())
                 .AddHeader("X-API-VERSION", "3.0")
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task use_header_version()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.My())
                 .AddHeader("X-API-VERSION", "2.0")
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task use_querystring_version()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest("api/users/my")
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

    }
}
