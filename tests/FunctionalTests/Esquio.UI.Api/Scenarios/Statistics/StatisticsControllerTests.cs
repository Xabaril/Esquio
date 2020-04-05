using Esquio.UI.Api.Shared.Models.Statistics.Configuration;
using Esquio.UI.Api.Shared.Models.Statistics.Success;
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

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Statistics
{
    [Collection(nameof(AspNetCoreServer))]
    public class statistics_controller_should
    {
        private readonly ServerFixture _fixture;
        public statistics_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task get_configuration_statistics_response_unauthorized_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Statistics.Configuration())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_configuration_statistics_response_configuration_statistics_when_success()
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
              .CreateRequest(ApiDefinitions.V3.Statistics.Configuration())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var statistics = await response.Content
                .ReadAs<ConfigurationStatisticsResponse>();

            statistics.TotalProducts
                .Should().Be(1);

            statistics.TotalFeatures
                .Should().Be(1);

            statistics.TotalRings
                .Should().Be(0);

            statistics.TotalToggles
                .Should().Be(1);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_success_statistics_response_unauthorized_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Statistics.Success())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_success_statistics_response_configuration_statistics_when_success()
        {
            var permission = Builders.Permission()
              .WithReaderPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);


            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Statistics.Success())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var statistics = await response.Content
                .ReadAs<SuccessStatisticResponse>();

            statistics.PercentageSuccess
                .Should().Be(0);
        }
    }
}
