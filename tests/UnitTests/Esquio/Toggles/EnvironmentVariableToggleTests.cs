using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class environment_variable_toggle_should
    {
        private const string EnvironmentVariable = nameof(EnvironmentVariable);
        private const string Values = nameof(Values);

        [Fact]
        public async Task throw_if_environment_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore((_, __) => null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new EnvironmentVariableToggle(null)
                .IsActiveAsync(Constants.FeatureName, Constants.ProductName);
            });
        }

        [Fact]
        public async Task be_not_active_if_current_environmentvariable_does_not_have_a_valid_value()
        {
            var toggle = Build
                .Toggle<EnvironmentVariableToggle>()
                .AddOneParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                .AddOneParameter(Values, "Production")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new EnvironmentVariableToggle(store)
                .IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured()
        {
            var toggle = Build
                .Toggle<EnvironmentVariableToggle>()
                .AddOneParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                .AddOneParameter(Values, "Production")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production", EnvironmentVariableTarget.Process);

            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new EnvironmentVariableToggle(store)
                .IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_with_different_case()
        {
            var toggle = Build
                 .Toggle<EnvironmentVariableToggle>()
                 .AddOneParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                 .AddOneParameter(Values, "pRoDuCtIoN")
                 .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production", EnvironmentVariableTarget.Process);

            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new EnvironmentVariableToggle(store)
                .IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_in_a_list()
        {
            var toggle = Build
                  .Toggle<EnvironmentVariableToggle>()
                  .AddOneParameter(EnvironmentVariable, "ASPNETCORE_ENVIRONMENT")
                  .AddOneParameter(Values, "Production;Development;Testing")
                  .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing", EnvironmentVariableTarget.Process);

            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new EnvironmentVariableToggle(store)
                .IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeTrue();
        }
    }
}
