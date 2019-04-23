using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class UserNameToggleShould
    {
        [Fact]
        public async Task be_not_active_if_user_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1;user2");

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new UserNameToggle(null, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "user2");
            var userNameProvider = new DelegatedUserNameProviderService(() => null);

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var store = new DelegatedValueFeatureStore(_ => null);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_users_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1");
            var userNameProvider = new DelegatedUserNameProviderService(() => "user2");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_to_users_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1");
            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_non_sensitive_to_users_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1");
            var userNameProvider = new DelegatedUserNameProviderService(() => "UsEr1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_contained_on_users_parameters_value()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1;user2");
            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
    }
}
