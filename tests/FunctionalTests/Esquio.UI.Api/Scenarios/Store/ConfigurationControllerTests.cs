using Esquio;
using Esquio.UI.Api.Shared.Models.Configuration.Details;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Store
{
    [Collection(nameof(AspNetCoreServer))]
    public class store_controller_should
    {
        private readonly ServerFixture _fixture;
        public store_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task response_unauthorized_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Configuration.Get(productName: "fooproduct", featureName: "barfeature", deployment: EsquioConstants.DEFAULT_DEPLOYMENT_NAME))
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task response_ok_when_sucess()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Deployment()
                .WithName(EsquioConstants.DEFAULT_DEPLOYMENT_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Deployments.Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Configuration.Get(productName: "fooproduct", featureName: "barfeature", deployment: EsquioConstants.DEFAULT_DEPLOYMENT_NAME))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<DetailsConfigurationResponse>();

            content.Enabled
                .Should().BeFalse();

            content.FeatureName
                .Should().Be("barfeature");

            content.Toggles
                .Count
                .Should().BeGreaterThan(0);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                .Count
                .Should()
                .Be(1);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                ["Environments"]
                .ToString()
                .Should()
                .BeEquivalentTo("Development");
        }

        [Fact]
        [ResetDatabase]
        public async Task response_ok_and_use_default_deployment_if_not_specified()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultRing = Builders.Deployment()
                .WithName(EsquioConstants.DEFAULT_DEPLOYMENT_NAME)
                .WithDefault(true)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Deployments
                .Add(defaultRing);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultRing.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Configuration.Get(productName: "fooproduct", featureName: "barfeature", deployment: EsquioConstants.DEFAULT_DEPLOYMENT_NAME))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<DetailsConfigurationResponse>();

            content.Enabled
                .Should().BeFalse();

            content.FeatureName
                .Should().Be("barfeature");

            content.Toggles
                .Count
                .Should().BeGreaterThan(0);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                .Count
                .Should()
                .Be(1);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                ["Environments"]
                .ToString()
                .Should()
                .BeEquivalentTo("Development");
        }

        [Fact]
        [ResetDatabase]
        public async Task response_ok_and_use_deployment_if_not_specified()
        {
            var permission = Builders.Permission()
             .WithManagementPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var defaultDeployment = Builders.Deployment()
                .WithName(EsquioConstants.DEFAULT_DEPLOYMENT_NAME)
                .WithDefault(true)
                .Build();

            var productionDeployment = Builders.Deployment()
                .WithName("Production")
                .WithDefault(false)
                .Build();

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            product.Deployments
                .Add(defaultDeployment);

            product.Deployments
                .Add(productionDeployment);

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.EnvironmentToggle,Esquio")
              .Build();

            var defaultDeploymentParameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .WithRingName(defaultDeployment.Name)
                .Build();

            var productionDeploymentParameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Production")
                .WithRingName(productionDeployment.Name)
                .Build();

            toggle.Parameters
                .Add(defaultDeploymentParameter);

            toggle.Parameters
                .Add(productionDeploymentParameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
              .CreateRequest(ApiDefinitions.V5.Configuration.Get(productName: "fooproduct", featureName: "barfeature", deployment: productionDeployment.Name))
              .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
              .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content
                .ReadAs<DetailsConfigurationResponse>();

            content.Enabled
                .Should().BeFalse();

            content.FeatureName
                .Should().Be("barfeature");

            content.Toggles
                .Count
                .Should().BeGreaterThan(0);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                .Count
                .Should()
                .Be(1);

            content.Toggles
                ["Esquio.Toggles.EnvironmentToggle,Esquio"]
                ["Environments"]
                .ToString()
                .Should()
                .BeEquivalentTo("Production");
        }
    }
}
