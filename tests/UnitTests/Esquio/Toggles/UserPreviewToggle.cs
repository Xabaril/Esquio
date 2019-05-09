using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class UserPreviewToggleShould
    {
        private const string PreviewUsers = nameof(PreviewUsers);
        private const string EnabledUsers = nameof(EnabledUsers);

        [Fact]
        public async Task be_not_active_if_user_provider_is_null()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(EnabledUsers, "user1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new UserPreviewToggle(null, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);
            });
        }
        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(EnabledUsers, "user2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => null);

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_preview_users_parameter_is_not_configured()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(EnabledUsers, "user2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_enabled_users_parameter_is_not_configured()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(PreviewUsers, "user2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_preview_users_parameter_value()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(PreviewUsers, "User3;User1")
                .AddOneParameter(EnabledUsers, "User3;User2")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User2");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_enabled_users_parameter_value()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(PreviewUsers, "User3;User2")
                .AddOneParameter(EnabledUsers, "User3;User1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User2");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_user_is_on_preview_and_enabled_users_parameter_value()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(PreviewUsers, "User3;User1")
                .AddOneParameter(EnabledUsers, "User3;User1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_with_different_case_is_on_preview_and_enabled_users_parameter_value()
        {
            var toggle = Build
                .Toggle<UserPreviewToggle>()
                .AddOneParameter(PreviewUsers, "User3;User1")
                .AddOneParameter(EnabledUsers, "User3;User1")
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "UsEr1");

            var active = await new UserPreviewToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);
            active.Should().BeTrue();
        }
    }
}
