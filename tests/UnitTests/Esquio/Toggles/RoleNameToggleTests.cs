using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class RoleNameToggleShould
    {
        [Fact]
        public async Task be_not_active_if_role_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "rol1;rol2");

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new RoleNameToggle(null, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            });
        }
        [Fact]
        public async Task be_not_active_if_role_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "rol1");
            var roleNameProvider = new DelegatedRoleNameProviderService(() => null);

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var store = new DelegatedValueFeatureStore(_ => null);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "rol1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_role_is_not_contained_on_roles_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "user2");
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "user1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_to_roles_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "rol1");
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "rol1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_non_sensitive_to_roles_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "rol1");
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "RoL1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "rol1;rol2");
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "rol1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            active.Should().BeTrue();
        }
    }
}
