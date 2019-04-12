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
    public class UserNameToggleShould
    {
        [Fact]
        public async Task be_not_active_if_user_provider_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
                .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user1;user2"))
                .Build();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await new UserNameToggle().IsActiveAsync(featureContext);
            });
        }
        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user2"))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => null))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => null))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user2"))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_to_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user1"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_non_sensitive_to_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user1"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "UsEr1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_contained_on_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedValueFeatureStore>(new DelegatedValueFeatureStore(_ => "user1;user2"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
    }
}
