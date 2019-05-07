using Esquio.Model;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class EnvironmentToggleShould
    {
        private const string Development = "development";
        private const string Production = "production";
        private const string Environments = nameof(Environments);

        [Fact]
        public async Task throw_if_environment_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore((_, __) => null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new EnvironmentToggle(null, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task throw_if_store_provider_is_null()
        {
            var provider = new DelegatedEnvironmentNameProviderService(() => string.Empty);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new EnvironmentToggle(provider, null).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task be_not_active_if_current_environment_is_null()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, $"{Development}:{Production}")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => null);

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_value_is_null()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => Development);

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_current_environment_is_not_configured()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Production)
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var store = new DelegatedValueFeatureStore((_,__) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => Development);

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Development)
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "development");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured_with_different_case()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Development)
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured_in_a_list()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, $"{Development};{Production}")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
    }
}
