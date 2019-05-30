using Esquio.UI.Api.Features.Flags.Details;
using Esquio.UI.Api.Features.Flags.List;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(productId: -1, featureId: 1))
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(productId: 1, featureId: -1))
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(productId: 10000, featureId: 1))
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, featureId: 1))
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, featureId: 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }
        [Fact]
        [ResetDatabase]
        public async Task delete_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle1 = Builders.Toggle()
              .WithType("toggle#1")
              .Build();

            feature.Toggles.Add(toggle1);

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Delete(product.Id, feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        [Fact]
        [ResetDatabase]
        public async Task delete_response_ok_when_product_and_feature_exist_and_feature_toggles_is_empty()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Delete(product.Id, feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        [Fact]
        [ResetDatabase]
        public async Task rollout_response_ok_when_product_and_feature_exist_and_featre_toggles_is_not_empty()
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(product.Id, featureId: 1))
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(productId: -1, featureId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(productId: 1, featureId: 1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
        [Fact]
        public async Task delete_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(productId: 1, featureId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_not_found_if_specified_product_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(productId: 1, featureId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
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
                   .CreateRequest(ApiDefinitions.V1.Flags.Delete(product.Id, featureId: 1))
                   .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                   .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_response_if_feature_exist()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
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

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(product.Id, feature.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsFlagResponse>();

            content.Id
                .Should()
                .Be(feature.Id);

            content.Name
                .Should()
                .BeEquivalentTo(feature.Name);

            content.Enabled
                .Should()
                .Be(feature.Enabled);

            content.ProductName
                .Should()
                .BeEquivalentTo(product.Name);

            content.Toggles
                .Should()
                .ContainEquivalentOf("toggle-type-1");

            content.Toggles
                .Should()
                .ContainEquivalentOf("toggle-type-2");
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_bad_request_if_product_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(productId: 10000, featureId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_bad_request_if_feature_not_exist()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Get(product.Id, featureId: 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task get_response_not_found_if_product_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(-1, 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task get_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(productId: 1, featureId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async Task get_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(productId: 1, featureId: 1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task list_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(1))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task list_response_notfound_when_productid_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(productId: -1))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_default_skip_take_values()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("feature#2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(product.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListFlagResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(2);

            content.PageIndex
                .Should().Be(0);

            content.Result
                .First().Name
                .Should().BeEquivalentTo("feature#1");

            content.Result
                .Last().Name
                .Should().BeEquivalentTo("feature#2");
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_specific_skip_take_values()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("feature#2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(product.Id, pageIndex: 1, pageCount: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListFlagResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(1);

            content.Result
                .Single().Name
                .Should().BeEquivalentTo("feature#2");
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_data()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();


            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(product.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListFlagResponse>();

            content.Total
                .Should().Be(0);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(0);

            content.Result
                .Count
                .Should().Be(0);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_page_data()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("feature#2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.List(product.Id, pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListFlagResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(10);

            content.Result
                .Count
                .Should().Be(0);
        }
    }
}
