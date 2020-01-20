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
    public class username_toggle_should
    {
        private const string Users = nameof(Users);

        [Fact]
        public void be_not_active_if_user_provider_is_null()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1;user2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                new UserNameToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var userNameProvider = new DelegatedUserNameProviderService(() => null);

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
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
                .Toggle<UserNameToggle>()
                .AddOneParameter("Users","SomeUser")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var userNameProvider = new DelegatedUserNameProviderService(() => "user2");

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_RING_NAME,
                  toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_user_is_equal_to_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_RING_NAME,
                  toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_user_is_equal_non_sensitive_to_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var userNameProvider = new DelegatedUserNameProviderService(() => "UsEr1");

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_RING_NAME,
                  toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_user_is_contained_on_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddOneParameter(Users, "user1;user2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var userNameProvider = new DelegatedUserNameProviderService(() => "user1");

            var active = await new UserNameToggle(userNameProvider).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_RING_NAME,
                  toggle));

            active.Should().BeTrue();
        }
    }
}
