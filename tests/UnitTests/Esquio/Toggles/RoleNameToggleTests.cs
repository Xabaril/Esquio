using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
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
                .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "rol1;rol2"))
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
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "rol1"))
              .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => null))
              .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => null))
              .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "rol1"))
              .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_role_is_not_contained_on_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user2"))
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
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "rol1"))
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
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "rol1"))
               .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "RoL1"))
               .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "rol1;rol2"))
               .WithService<IRoleNameProviderService, DelegatedRoleNameProviderService>(new DelegatedRoleNameProviderService(() => "rol1"))
               .Build();

            var active = await new RoleNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
    }
}
