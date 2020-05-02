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
                .AddParameter(Roles, "role1")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity());

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
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
                .AddParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultRoleClaimType, "userole") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
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
                .AddParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_not_active_if_role_is_not_equal_case_sensitive_to_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultRoleClaimType, "AdMiN") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_role_is_contained_on_roles_parameters_value()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddParameter(Roles, "admin;user")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_when_role_type_is_different_from_default_role_type()
        {
            var toggle = Build
                .Toggle<RoleNameToggle>()
                .AddParameter(Roles, "admin")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim("role", "admin") }, "cookies", nameType: "name", roleType: "role" ));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new RoleNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}
