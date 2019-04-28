using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class EnvironmentToggleShould
    {
        [Fact]
        public async Task throw_if_role_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => string.Empty);

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
            var store = new DelegatedValueFeatureStore(_ => "development;production");
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => null);

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_value_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => null);
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "development");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_current_environment_is_not_configured()
        {
            var store = new DelegatedValueFeatureStore(_ => "production");
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "development");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured()
        {
            var store = new DelegatedValueFeatureStore(_ => "development");
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "development");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured_with_different_case()
        {
            var store = new DelegatedValueFeatureStore(_ => "development");
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_current_environment_is_configured_in_a_list()
        {
            var store = new DelegatedValueFeatureStore(_ => "production;development");
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
    }
}
