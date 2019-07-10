using Esquio.UI.Api.Features.Flags.Add;
using Esquio.UI.Api.Features.Flags.Details;
using Esquio.UI.Api.Features.Flags.List;
using Esquio.UI.Api.Features.Flags.Update;
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
        public async Task rollback_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: 1))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
        [Fact]
        public async Task rollback_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_bad_request_if_feature_not_exist()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_ok_when_product_and_feature_exist_and_feature_toggles_is_empty()
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_is_idempotent()
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var product = Builders.Product()
                .WithName("product#2")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("Esquio.Toggles.OnToggle")
                .Build();

            var toggle2 = Builders.Toggle()
                .WithType("CustomToggle")
                .Build();

            feature.Toggles
                .Add(toggle1);

            feature.Toggles
                .Add(toggle2);

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollback(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task rollout_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: 1))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task rollout_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: 1))
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_is_idempotent()
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
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_set_feature_as_enabled()
        {
            var product = Builders.Product()
                .WithName("product#2")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature")
                .WithEnabled(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.Flags.Get(featureId: feature.Id))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            var details = await response.Content
                .ReadAs<DetailsFlagResponse>();

            details.Enabled
                .Should().BeTrue();
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var product = Builders.Product()
                .WithName("product#2")
                .Build();

            var feature = Builders.Feature()
                .WithName("feature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("Esquio.Toggles.OffToggle")
                .Build();

            var toggle2 = Builders.Toggle()
                .WithType("CustomToggle")
                .Build();

            feature.Toggles
                .Add(toggle1);

            feature.Toggles
                .Add(toggle2);

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Rollout(featureId: feature.Id))
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
              .CreateRequest(ApiDefinitions.V1.Flags.Delete(feature.Id))
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
              .CreateRequest(ApiDefinitions.V1.Flags.Delete(feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        

        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(featureId: 1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
        [Fact]
        public async Task delete_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Delete(featureId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

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
                   .CreateRequest(ApiDefinitions.V1.Flags.Delete(featureId: 1))
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(feature.Id))
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

            content.Description
                .Should()
                .BeEquivalentTo(feature.Description);

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
        public async Task get_response_bad_request_if_feature_not_exist()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Flags.Get(featureId: 1))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task get_response_not_found_if_feature_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(featureId: -1))
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
                  .CreateRequest(ApiDefinitions.V1.Flags.Get(featureId: 1))
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
                .Count
                .Should().Be(2);

            content.Result
                .First()
                .Name.Should().BeEquivalentTo(feature1.Name);

            content.Result
               .First()
               .Description.Should().BeEquivalentTo(feature1.Description);

            content.Result
               .First()
               .Enabled.Should().Be(feature1.Enabled);
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

        [Fact]
        public async Task update_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Update())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
        [Fact]
        public async Task update_response_badrequest_if_name_is_greater_than_200()
        {
            var updateFlagRequest = new UpdateFlagRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
                FlagId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Update())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task update_response_badrequest_if_description_is_greater_than_2000()
        {
            var updateFlagRequest = new UpdateFlagRequest()
            {
                Name = "name",
                Description = new string('c', 2001),
                Enabled = true,
                FlagId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Update())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task update_response_badrequest_if_flag_does_not_exist()
        {
            var updateFlagRequest = new UpdateFlagRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
                FlagId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Update())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task update_response_ok_when_create_the_feature()
        {
            var product = Builders.Product()
              .WithName("product#1")
              .Build();

            var feature = Builders.Feature()
                .WithName("feature1")
                .WithEnabled(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var updateFlagRequest = new UpdateFlagRequest()
            {
                Name = "feature#1",
                Description = "description",
                Enabled = true,
                FlagId = feature.Id
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Update())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }


        [Fact]
        public async Task add_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task add_response_badrequest_if_name_is_greater_than_200()
        {
            var addFlagRequest = new AddFlagRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
                ProductId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task add_response_badrequest_if_description_is_greater_than_2000()
        {
            var addFlagRequest = new AddFlagRequest()
            {
                Name = "name",
                Description = new string('c', 2001),
                Enabled = true,
                ProductId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_product_does_not_exist()
        {
            var addFlagRequest = new AddFlagRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
                ProductId = 1
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_feature_with_the_same_name_already_exist_on_same_product()
        {
            var product = Builders.Product()
                .WithName("product#1")
                .Build();

            var feature1 = Builders.Feature()
                .WithName("feature#1")
                .Build();

            product.Features
                .Add(feature1);

            await _fixture.Given
                .AddProduct(product);

            var addFlagRequest = new AddFlagRequest()
            {
                Name = "feature#1",
                Description = "description",
                Enabled = true,
                ProductId = product.Id
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_created_if_feature_with_the_same_name_already_exist_on_different_product()
        {
            var product1 = Builders.Product()
                .WithName("product#1")
                .Build();

            var product2 = Builders.Product()
                .WithName("product#2")
                .Build();

            var feature1 = Builders.Feature()
                .WithName("feature#1")
                .Build();

            product1.Features
                .Add(feature1);

            await _fixture.Given
                .AddProduct(product1,product2);

            var addFlagRequest = new AddFlagRequest()
            {
                Name = "feature#1",
                Description = "description",
                Enabled = true,
                ProductId = product2.Id
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_ok_when_create_the_feature()
        {
            var product = Builders.Product()
              .WithName("product#1")
              .Build();

            await _fixture.Given
                .AddProduct(product);

            var addFlagRequest = new AddFlagRequest()
            {
                Name = "feature#1",
                Description = "description",
                Enabled = true,
                ProductId = product.Id
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Flags.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }
    }
}
