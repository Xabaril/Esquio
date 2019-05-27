using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
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

        [Fact]
        public async Task response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Rollout(1, 1))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task response_not_found_if_product_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(-1, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(1, -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task response_bad_request_if_product_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(10000, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task response_bad_request_if_feature_not_exist()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task response_ok_if_feature_toggles_is_empty()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);
           

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }
    }
}
