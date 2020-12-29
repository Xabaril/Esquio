using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Audit.List;
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
                .CreateRequest(ApiDefinitions.V5.Audit.List())
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
               .WithReaderPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Audit.List())
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
                .WithManagementPermission()
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
                .WithFeatureName(feature.Name)
                .WithProductName(product.Name)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            await _fixture.Given
                .AddHistory(history);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Audit.List())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListAuditResponseDetail>>();

            content.PageIndex.Should().Be(0);
            content.Count.Should().Be(3);
            content.Total.Should().Be(3); //add feature already add new history also and product a deployment
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_skip_take_values()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
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
                .WithFeatureName(feature.Name)
                .WithProductName(product.Name)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            var history2 = Builders.History()
                .WithFeatureName(feature.Name)
                .WithProductName(product.Name)
                .WithOldValues("")
                .WithNewValues("{environments:development}")
                .Build();

            await _fixture.Given
                .AddHistory(history1,history2);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Audit.List(pageIndex:1,pageCount:1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListAuditResponseDetail>>();

            content.PageIndex.Should().Be(1);
            content.Count.Should().Be(1);
            content.Total.Should().Be(4); //add feature already add new history also
        }
    }
}
