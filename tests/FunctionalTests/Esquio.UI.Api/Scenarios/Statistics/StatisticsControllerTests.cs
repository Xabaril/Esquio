using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Statistics.Configuration;
using Esquio.UI.Api.Shared.Models.Statistics.Plot;
using Esquio.UI.Api.Shared.Models.Statistics.Success;
using Esquio.UI.Api.Shared.Models.Statistics.TopFeatures;
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

            statistics.TotalDeployments
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
        [Fact]
        [ResetDatabase]
        public async Task get_success_statistics_response_configuration_statistics_when_success_many_items()
        {
            var permission = Builders.Permission()
              .WithReaderPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);
            await _fixture.Given.AddMetric(SampleMetrics());

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
                .Should().Be(41);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_top5_statistics_response()
        {
            var permission = Builders.Permission()
              .WithReaderPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);
            await _fixture.Given.AddMetric(SampleMetrics());

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Statistics.Top())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var topFeatures = await response.Content
                .ReadAs<TopFeaturesStatisticsResponse>();

            topFeatures.TopFeaturesDetails.Count().Should().Be(5);
            topFeatures.TopFeaturesDetails.First().FeatureName.Should().Be("Feature2");
            topFeatures.TopFeaturesDetails.First().Requests.Should().Be(4);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_plot_statistics_response()
        {
            var permission = Builders.Permission()
              .WithReaderPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);
            await _fixture.Given.AddMetric(SampleMetrics());

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V3.Statistics.Plot())
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var plotResponse = await response.Content
                .ReadAs<PlotStatisticsResponse>();

            plotResponse.Should().NotBeNull();

            plotResponse.Points.Count().Should().BeInRange(2880, 2885);
        }
        private MetricEntity[] SampleMetrics(){
            return new [] {
                Builders.Metric().WithKind("Success").WithFeatureName("Feature1").WithDateTime(DateTime.Now.AddHours(-5)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature1").WithDateTime(DateTime.Now.AddHours(-4)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature1").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature2").WithDateTime(DateTime.Now.AddHours(-5)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature2").WithDateTime(DateTime.Now.AddHours(-4)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature2").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature2").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature3").WithDateTime(DateTime.Now.AddHours(-5)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature3").WithDateTime(DateTime.Now.AddHours(-4)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature3").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature4").WithDateTime(DateTime.Now.AddHours(-5)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature4").WithDateTime(DateTime.Now.AddHours(-4)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature4").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature5").WithDateTime(DateTime.Now.AddHours(-5)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature5").WithDateTime(DateTime.Now.AddHours(-4)).Build(),
                Builders.Metric().WithKind("Failure").WithFeatureName("Feature5").WithDateTime(DateTime.Now.AddHours(-3)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature6").WithDateTime(DateTime.Now.AddHours(-22)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature6").WithDateTime(DateTime.Now.AddHours(-24)).Build(),
                Builders.Metric().WithKind("Success").WithFeatureName("Feature6").WithDateTime(DateTime.Now.AddHours(-25)).Build(),
            };
            
        }
        
    }
}
