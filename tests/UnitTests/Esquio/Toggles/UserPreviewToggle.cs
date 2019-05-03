using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class UserPreviewToggleShould
    {
        [Fact]
        public async Task be_not_active_if_user_provider_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "user1;user2");

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new UserPreviewToggle(null, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            });
        }
        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var store = new DelegatedValueFeatureStore(_ => "user2");
            var userNameProvider = new DelegatedUserNameProviderService(() => null);

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_preview_users_parameter_is_not_configured()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "EnabledUsers")
                    return "User1";
                else
                    return null;
            });

            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_enabled_users_parameter_is_not_configured()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "PreviewUsers")
                    return "User1";
                else
                    return null;
            });

            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_preview_users_parameter_value()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "PreviewUsers")
                    return "User3;User1";
                else
                    return "User3;User2";
            });
            var userNameProvider = new DelegatedUserNameProviderService(() => "User2");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_enabled_users_parameter_value()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "PreviewUsers")
                    return "User3;User2";
                else
                    return "User3;User1";
            });
            var userNameProvider = new DelegatedUserNameProviderService(() => "User2");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_user_is_on_preview_and_enabled_users_parameter_value()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "PreviewUsers")
                    return "User3;User1";
                else
                    return "User3;User1";
            });
            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_with_different_case_is_on_preview_and_enabled_users_parameter_value()
        {
            var store = new DelegatedValueFeatureStore(name =>
            {
                if (name == "PreviewUsers")
                    return "User3;User1";
                else
                    return "User3;User1";
            });
            var userNameProvider = new DelegatedUserNameProviderService(() => "UsEr1");

            var active = await new UserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);
            active.Should().BeTrue();
        }
    }
}
