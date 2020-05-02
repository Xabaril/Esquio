using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Features.Add;
using Esquio.UI.Api.Shared.Models.Features.Details;
using Esquio.UI.Api.Shared.Models.Features.List;
using Esquio.UI.Api.Shared.Models.Features.Update;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Features
{
    [Collection(nameof(AspNetCoreServer))]
    public class feature_controller_should
    {
        private readonly ServerFixture _fixture;
        public feature_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task archive_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Archive(productName: "fooproduct", featureName: "barfeature"))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task archive_response_bad_request_if_feature_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("product")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Archive(product.Name, featureName: "barfeature"))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task archive_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Archive(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task archive_response_ok_when_product_and_feature_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .WithArchived(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Archive(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task rollback_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Rollback(productName: "fooproduct", featureName: "barfeature"))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_bad_request_if_feature_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("product")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, featureName: "barfeature"))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_is_idempotent()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);

            response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollback_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);
            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("Esquio.Toggles.FromToToggle")
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
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        [Fact]
        [ResetDatabase]
        public async Task rollback_set_feature_as_not_enabled()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .WithEnabled(true)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollback(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);

            response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Get(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            var details = await response.Content
                .ReadAs<DetailsFeatureResponse>();

            details.Enabled
                .Should().BeFalse();
        }

        [Fact]
        public async Task rollout_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Rollout(productName: "fooproduct", featureName: "barfeature"))
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_response_bad_request_if_feature_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, featureName: "barfeature"))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_is_idempotent()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);

            response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_set_feature_as_enabled()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .WithEnabled(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);

            response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Get(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            var details = await response.Content
                .ReadAs<DetailsFeatureResponse>();

            details.Enabled
                .Should().BeTrue();
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .WithEnabled(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task rollout_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
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
              .CreateRequest(ApiDefinitions.V3.Features.Rollout(product.Name, feature.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        [Fact]
        [ResetDatabase]
        public async Task delete_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Delete(productName: "fooproduct", featureName: "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
        [Fact]
        [ResetDatabase]
        public async Task delete_response_ok_when_product_and_feature_exist_and_feature_toggles_is_not_empty()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("toggle")
                .Build();

            feature.Toggles.Add(toggle1);

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);


            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Delete(product.Name, feature.Name))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Delete(product.Name, feature.Name))
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
                .CreateRequest(ApiDefinitions.V3.Features.Delete(productName: "fooproduct", featureName: "barfeature"))
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_not_found_if_specified_feature_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Delete(product.Name, featureName: "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_forbidden_if_user_is_not_authorized()
        {

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Get(productName: "fooproduct", featureName: "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_toggle_details_response_if_feature_exist_and_is_configured()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle1 = Builders.Toggle()
                .WithType("Esquio.Toggles.FromToToggle,Esquio")
                .Build();

            var toggle2 = Builders.Toggle()
                .WithType("Esquio.Toggles.EnvironmentVariableToggle,Esquio")
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
                .CreateRequest(ApiDefinitions.V3.Features.Get(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsFeatureResponse>();

            content.Name
                .Should()
                .BeEquivalentTo(feature.Name);

            content.Description
                .Should()
                .BeEquivalentTo(feature.Description);

            content.Enabled
                .Should()
                .Be(feature.Enabled);

            content.Toggles
                .Should()
                .ContainEquivalentOf(new ToggleDetail
                {
                    FriendlyName = "Between dates",
                    Type = toggle1.Type,
                    Parameters = new List<ParameterDetail>()
                });

            content.Toggles
               .Should()
               .ContainEquivalentOf(new ToggleDetail
               {
                   FriendlyName = "Environment Variable",
                   Type = toggle2.Type,
                   Parameters = new List<ParameterDetail>()
               });
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_bad_request_if_feature_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Get(product.Name, "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }


        [Fact]
        public async Task get_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Get(productName: "fooproduct", featureName: "barfeature"))
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task list_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.List(productName: "fooproduct"))
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_default_skip_take_values()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("barfeature1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("barfeature2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.List(product.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListFeatureResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(2);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .First().Name
                .Should().BeEquivalentTo(feature1.Name);

            content.Items
                .Last().Name
                .Should().BeEquivalentTo(feature2.Name);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_skip_archived_features()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("barfeature1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("barfeature2")
                .WithArchived(true)
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.List(product.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListFeatureResponseDetail>>();

            content.Total
                .Should().Be(1);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .First().Name
                .Should().BeEquivalentTo(feature1.Name);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_specific_skip_take_values()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature1 = Builders.Feature()
                .WithName("bar1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("bar2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.List(product.Name, pageIndex: 1, pageCount: 1))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListFeatureResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(1);

            content.Items
                .Single().Name
                .Should().BeEquivalentTo(feature2.Name);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_data()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("bar1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("bar2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);


            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.List(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListFeatureResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(2);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .Count
                .Should().Be(2);

            content.Items
                .First()
                .Name.Should().BeEquivalentTo(feature1.Name);

            content.Items
               .First()
               .Description.Should().BeEquivalentTo(feature1.Description);

            content.Items
               .First()
               .Enabled.Should().Be(feature1.Enabled);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_forbidden_when_user_is_not_authorized()
        {
            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("bar1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("bar2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);


            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.List(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_page_data()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature1 = Builders.Feature()
                .WithName("bar1")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("bar2")
                .Build();

            product.Features
                .Add(feature1);

            product.Features
               .Add(feature2);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.List(product.Name, pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListFeatureResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(10);

            content.Items
                .Count
                .Should().Be(0);
        }

        [Fact]
        public async Task update_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Update(productName: "fooproduct", featureName: "barfeature"))
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_badrequest_if_name_is_greater_than_200()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var updateFlagRequest = new UpdateFeatureRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Update(productName: "fooproduct", featureName: "barfeature"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_badrequest_if_description_is_greater_than_2000()
        {
            var permission = Builders.Permission()
             .WithManagementPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var updateFlagRequest = new UpdateFeatureRequest()
            {
                Name = "name",
                Description = new string('c', 2001),
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Update(productName: "fooproduct", featureName: "barfeature"))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var updateFlagRequest = new UpdateFeatureRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Update(productName: "fooproduct", featureName: "barfeature"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_ok_when_modify_the_feature()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
              .WithName("fooproduct")
              .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .WithEnabled(false)
                .Build();

            product.Features.Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var updateFlagRequest = new UpdateFeatureRequest()
            {
                Name = "bar2",
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Update(product.Name, feature.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(updateFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task add_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(productName: "fooproduct"))
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_name_is_greater_than_200()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(productName: "fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_description_is_greater_than_2000()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = "barfeature",
                Description = new string('c', 2001),
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(productName: "fooproduct"))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = new string('c', 201),
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(productName: "fooproduct"))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = feature.Name,
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(product.Name))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product1 = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var product2 = Builders.Product()
                .WithName("barfeature")
                .Build();

            var feature1 = Builders.Feature()
                .WithName("foofeature")
                .Build();

            product1.Features
                .Add(feature1);

            await _fixture.Given
                .AddProduct(product1, product2);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = feature1.Name,
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(product2.Name))
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
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
              .WithName("fooproduct")
              .Build();

            await _fixture.Given
                .AddProduct(product);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = "barfeature",
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Features.Add(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var addFlagRequest = new AddFeatureRequest()
            {
                Name = "barfeature",
                Description = "description",
                Enabled = true,
            };

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Features.Add(product.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(addFlagRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
    }
}
