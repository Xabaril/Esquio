using Esquio.UI.Api.Features.Tags.Add;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
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
        public async Task not_allow_to_untagged_features_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Delete("tag", 1))
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_tag_features_with_the_same()
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
              .CreateRequest(ApiDefinitions.V1.Tags.Delete(tag.Name, feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_bad_request_when_the_feature_is_untagged()
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
              .CreateRequest(ApiDefinitions.V1.Tags.Delete("tag", feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_tag_features()
        {
            var tag = "tag";
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

            var request = new AddTagRequest(tag, feature.Id);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Add())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_tag_features_when_feature_does_not_exists()
        {
            var tag = "tag";
            var request = new AddTagRequest(tag, 1);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Add())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task not_allow_to_tag_features_when_feature_has_been_tagged_with_the_same_tag()
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
            var request = new AddTagRequest(tag.Name, feature.Id);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Add())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .PostAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
    }
}
