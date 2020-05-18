using Esquio;
using Esquio.Abstractions;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class environment_variable_toggle_should
    {
        private const string EnvironmentVariable = nameof(EnvironmentVariable);
        private const string Values = nameof(Values);


        [Fact]
        public async Task be_not_active_if_current_environmentvariable_does_not_have_a_valid_value()
        {
            var toggle = Build
                .Toggle<EnvironmentVariableToggle>()
                .AddParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                .AddParameter(Values, "Production")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var active = await new EnvironmentVariableToggle().IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                    toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured()
        {
            var toggle = Build
                .Toggle<EnvironmentVariableToggle>()
                .AddParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                .AddParameter(Values, "Production")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production", EnvironmentVariableTarget.Process);

            var active = await new EnvironmentVariableToggle().IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                    toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_with_different_case()
        {
            var toggle = Build
                 .Toggle<EnvironmentVariableToggle>()
                 .AddParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                 .AddParameter(Values, "pRoDuCtIoN")
                 .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production", EnvironmentVariableTarget.Process);

            

            var active = await new EnvironmentVariableToggle().IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                    toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_in_a_list()
        {
            var toggle = Build
                  .Toggle<EnvironmentVariableToggle>()
                  .AddParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                  .AddParameter(Values, "Production;Development;Testing")
                  .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing", EnvironmentVariableTarget.Process);

            var active = await new EnvironmentVariableToggle().IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                    toggle));

            active.Should().BeTrue();
        }
    }
}
