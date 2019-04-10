using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Seedwork.Builders;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class RoleNameToggleShould
    {
        [Fact]
        public async Task be_not_active_if_role_provider_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
                .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "rol1;rol2"))
                .Build();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await new RoleNameToggle().IsActiveAsync(featureContext);
            });
        }
        [Fact]
        public async Task be_not_active_if_role_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "rol1"))
              .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => null))
              .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => null))
              .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "rol1"))
              .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_role_is_not_contained_on_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user2"))
              .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "user1"))
              .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_to_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithFeatureName("feature")
               .WithApplicationName("application")
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "rol1"))
               .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "rol1"))
               .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_equal_non_sensitive_to_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithFeatureName("feature")
               .WithApplicationName("application")
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "rol1"))
               .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "RoL1"))
               .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "rol1;rol2"))
               .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "rol1"))
               .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        class DelegatedFeatureStore
            : IFeatureStore
        {
            Func<string> _getConfiguredRolesFunc;

            public DelegatedFeatureStore(Func<string> getConfiguredRolesFunc)
            {
                _getConfiguredRolesFunc = getConfiguredRolesFunc ?? throw new ArgumentNullException(nameof(getConfiguredRolesFunc));
            }
            public Task<object> GetParameterValueAsync<TToggle>(string application, string feature, string parameterName) where TToggle : IToggle
            {
                var configuredUsers = _getConfiguredRolesFunc();
                return Task.FromResult((object)configuredUsers);
            }
            public Task<bool> AddFeatureAsync(string application, string feature, bool enabled = false)
            {
                return Task.FromResult(true);
            }
            public Task<bool> AddToggleAsync<TToggle>(string application, string feature, IDictionary<string, object> parameterValues) where TToggle : IToggle
            {
                return Task.FromResult(true);
            }
        }
        class DelegatedRoleNameProviderService
            : IRoleNameProviderService
        {
            Func<string> _getCurrentRoleFunc;
            public DelegatedRoleNameProviderService(Func<string> getCurrentRoleFunc)
            {
                _getCurrentRoleFunc = getCurrentRoleFunc ?? throw new ArgumentNullException(nameof(getCurrentRoleFunc));
            }
            public Task<string> GetCurrentRoleNameAsync()
            {
                return Task.FromResult(_getCurrentRoleFunc());
            }
        }
    }
}
