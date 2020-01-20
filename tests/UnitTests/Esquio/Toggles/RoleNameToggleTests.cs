using Esquio;
using Esquio.Abstractions;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class rolename_toggle_should
    {
        private const string Roles = nameof(Roles);

        [Fact]
        public void throw_if_role_provider_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RoleNameToggle(null);
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

            var roleNameProvider = new DelegatedRoleNameProviderService(() => null);

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter("Roles","SomeRole")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var roleNameProvider = new DelegatedRoleNameProviderService(() => "role1");

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_role_is_not_contained_on_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var roleNameProvider = new DelegatedRoleNameProviderService(() => "user");

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
                           ToggleExecutionContext.FromToggle(
                               feature.Name,
                               EsquioConstants.DEFAULT_PRODUCT_NAME,
                               EsquioConstants.DEFAULT_RING_NAME,
                               toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_role_is_equal_to_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "admin");

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_role_is_equal_non_sensitive_to_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "AdMiN");

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddOneParameter(Roles, "admin;user")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var roleNameProvider = new DelegatedRoleNameProviderService(() => "user");

            var active = await new RoleNameToggle(roleNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}
