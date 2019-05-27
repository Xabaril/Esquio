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
        public async Task rollout_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Rollout(1, 1))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task rollout_response_not_found_if_product_is_not_positive_int()
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
        public async Task rollout_response_not_found_if_feature_is_not_positive_int()
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
        public async Task rollout_response_bad_request_if_product_not_exist()
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
        public async Task rollout_response_bad_request_if_feature_not_exist()
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
        public async Task rollout_response_ok_when_product_and_feature_exist_and_feature_toggles_is_empty()
        {
            var product = Builders.Product()
                .WithName("product#2")
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
        [Fact]
        [ResetDatabase]
        public async Task rollout_response_ok_when_product_and_feature_exist()
        {
            var product = Builders.Product()
                .WithName("product#2")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("toggle-type-1")
                .Build();

            var toggle2 = Builders.Toggle()
                .WithType("toggle-type-2")
                .Build();

            feature.Toggles
                .Add(toggle1);

            feature.Toggles
                .Add(toggle2);

            product.Features
                .Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }
        [Fact]
        public async Task delete_response_not_found_if_product_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(-1, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task delete_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(1, -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_not_found_if_specified_product_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(1, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        [ResetDatabase]
        public async Task delete_response_not_found_if_specified_feature_not_exist()
        {
            var product = Builders.Product()
               .WithName("product#2")
               .Build();

            await _fixture.Given.AddProduct(product);

           var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(product.Id, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
    }
}
