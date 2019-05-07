using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class RoleNameToggleShould
    {
        private const string Roles = nameof(Roles);

        [Fact]
        public async Task throw_if_role_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore((_,__) => null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new RoleNameToggle(null, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task throw_if_store_is_null()
        {
            var roleNameProvider = new DelegatedRoleNameProviderService(() => null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new RoleNameToggle(roleNameProvider, null).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task be_not_active_if_role_is_null()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "role1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => null);

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "role1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_role_is_not_contained_on_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "user2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "user1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_to_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "role1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "role1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_non_sensitive_to_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "role1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "RoLe1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "role1;role2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "role1");

            var active = await new RoleNameToggle(roleNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
    }
}
