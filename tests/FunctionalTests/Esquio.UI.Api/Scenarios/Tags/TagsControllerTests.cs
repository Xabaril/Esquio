using Esquio.UI.Api.Features.Tags.Add;
using Esquio.UI.Api.Features.Tags.List;
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
              .CreateRequest(ApiDefinitions.V1.Tags.Untag("tag", 1))
              .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_untag_features()
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
              .CreateRequest(ApiDefinitions.V1.Tags.Untag(tag.Name, feature.Id))
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
              .CreateRequest(ApiDefinitions.V1.Tags.Untag("tag", feature.Id))
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

            var request = new AddTagRequest(tag);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Tag(feature.Id))
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
            var request = new AddTagRequest(tag);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Tag(1))
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
            var request = new AddTagRequest(tag.Name);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V1.Tags.Tag(feature.Id))
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
              .CreateRequest(ApiDefinitions.V1.Tags.List(feature.Id))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<List<ListTagResponse>>();

            content.Should().HaveCount(1);
        }
    }
}
