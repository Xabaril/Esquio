using Esquio.UI.Api.Features.Audit.List;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Audit
{
    [Collection(nameof(AspNetCoreServer))]
    public class audit_controller_should
    {
        private readonly ServerFixture _fixture;
        public audit_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task list_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V2.Audit.List())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_forbidden_when_user_request_is_not_authorized()
        {
            var permission = Builders.Permission()
               .WithAllPrivilegesForDefaultIdentity()
               .WithManagementPermission(false)
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V2.Audit.List())
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_default_skip_take_values()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            //this also add new history automatically
            await _fixture.Given
                .AddProduct(product);

            await _fixture.Given
                .AddPermission(permission);

            var history = Builders.History()
                .WithFeatureId(feature.Id)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            await _fixture.Given
                .AddHistory(history);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V2.Audit.List())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListAuditResponse>();

            content.PageIndex.Should().Be(0);
            content.Count.Should().Be(2);
            content.Total.Should().Be(2); //add feature already add new history also
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_skip_take_values()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            //this add new history also
            await _fixture.Given
                .AddProduct(product);

            await _fixture.Given
                .AddPermission(permission);

            var history1 = Builders.History()
                .WithFeatureId(feature.Id)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            var history2 = Builders.History()
                .WithFeatureId(feature.Id)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            await _fixture.Given
                .AddHistory(history1,history2);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V2.Audit.List(pageIndex:1,pageCount:1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListAuditResponse>();

            content.PageIndex.Should().Be(1);
            content.Count.Should().Be(1);
            content.Total.Should().Be(3); //add feature already add new history also
        }
    }
}
