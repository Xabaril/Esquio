using Esquio.UI.Api.Shared.Models.Tags.Add;
using Esquio.UI.Api.Shared.Models.Tags.List;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Tags
{
    [Collection(nameof(AspNetCoreServer))]
    public class tags_controller_should
    {
        private readonly ServerFixture _fixture;
        public tags_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }
        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_untag_features_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Tags.Untag(productName: "fooproduct", featureName: "barfeature", tag: "peformance"))
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_untag_features()
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

            var tag = Builders.Tag()
                .Build();
            var featureTag = Builders.FeatureTag()
                .WithFeature(feature)
                .WithTag(tag)
                .Build();

            feature.Toggles.Add(toggle1);
            feature.FeatureTags.Add(featureTag);
            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Tags.Untag(product.Name, feature.Name, tag.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_untag_a_feature_when_it_has_not_been_previously_tagged_with_the_tag()
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
              .CreateRequest(ApiDefinitions.V3.Tags.Untag(product.Name, feature.Name, tag: "performance"))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task tag_feature_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var tag = "tag";

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

            var request = new AddTagRequest(tag);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_tag_features_with_default_color()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var tag = "tag";

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

            var request = new AddTagRequest(tag);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_tag_features_with_specified_color()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var tag = "tag";

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

            var request = new AddTagRequest(tag,"#FF0022");

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }


        [Fact]
        [ResetDatabase]
        public async Task response_bad_request_when_add_tag_features_with_bad_formatted_color()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var tag = "tag";

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

            var request = new AddTagRequest(tag, "#nonvalid");

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_tag_features_with_existing_tags()
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
                .WithName("barfeatureone")
                .Build();

            var feature2 = Builders.Feature()
                .WithName("barfeaturetwo")
                .Build();

            var tag = Builders.Tag()
                .WithName("performance")
                .Build();

            var featureTag = Builders.FeatureTag()
                .WithFeature(feature1)
                .WithTag(tag)
                .Build();

            feature1.FeatureTags.Add(featureTag);
            product.Features.Add(feature1);
            product.Features.Add(feature2);

            await _fixture.Given.AddProduct(product);

            var request = new AddTagRequest("performance");

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature2.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_tag_features_when_feature_does_not_exists()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var tag = "tag";
            var request = new AddTagRequest(tag);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(productName: "fooproduct", featureName: "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_tag_features_when_it_has_been_previously_tagged_with_the_same_tag()
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

            var tag = Builders.Tag()
                .Build();

            var featureTag = Builders.FeatureTag()
                .WithFeature(feature)
                .WithTag(tag)
                .Build();

            feature.Toggles.Add(toggle1);
            feature.FeatureTags.Add(featureTag);
            product.Features.Add(feature);

            await _fixture.Given.AddProduct(product);

            var request = new AddTagRequest(tag.Name);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.Tag(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_back_the_list_of_tags_of_a_feature()
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

            var tagPerformance = Builders.Tag()
                .WithName("performance")
                .Build();

            var tagUsability = Builders.Tag()
                .WithName("usability")
                .Build();

            var featureTagPerformance = Builders.FeatureTag()
                .WithFeature(feature)
                .WithTag(tagPerformance)
                .Build();

            var featureTagUsability = Builders.FeatureTag()
                .WithFeature(feature)
                .WithTag(tagUsability)
                .Build();

            feature.Toggles.Add(toggle1);
            feature.FeatureTags.Add(featureTagPerformance);
            feature.FeatureTags.Add(featureTagUsability);

            product.Features.Add(feature);
            await _fixture.Given.AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.List(product.Name, feature.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<List<TagResponseDetail>>();

            content.Should().HaveCount(2);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_forbidden_if_user_is_unauthorized()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Tags.List(productName: "fooproduct", featureName: "barfeature"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
    }
}
