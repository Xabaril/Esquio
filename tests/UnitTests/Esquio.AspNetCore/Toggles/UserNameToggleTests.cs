using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class username_toggle_should
    {
        private const string Users = nameof(Users);

        [Fact]
        public void be_not_active_if_user_provider_is_null()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddParameter(Users, "user1;user2")
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
                .AddParameter(Users, "user")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity());

            var contextAccessor = new FakeHttpContextAccessor(context);
           
            var active = await new UserNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeFalse();
        }

      

        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "user2") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserNameToggle(contextAccessor).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                  toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_user_is_equal_to_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "user1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserNameToggle(contextAccessor).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                  toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_user_is_equal_non_sensitive_to_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddParameter(Users, "user1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "UsEr1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserNameToggle(contextAccessor).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                  toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_user_is_contained_on_users_parameters_value()
        {
            var toggle = Build
                .Toggle<UserNameToggle>()
                .AddParameter(Users, "user1;user2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "user1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserNameToggle(contextAccessor).IsActiveAsync(
              ToggleExecutionContext.FromToggle(
                  feature.Name,
                  EsquioConstants.DEFAULT_PRODUCT_NAME,
                  EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                  toggle));

            active.Should().BeTrue();
        }
    }
}
