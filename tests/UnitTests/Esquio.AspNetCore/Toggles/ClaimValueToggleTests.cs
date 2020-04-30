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
    public class claimvalue_toggle_tests
    {
        [Fact]
        public void throw_if_store_service_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var accessor = new FakeHttpContextAccesor();
                new ClaimValueToggle(null, accessor);
            });
        }

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var store = new DelegatedValueFeatureStore((_, __) => null);
                new ClaimValueToggle(store, null);
            });
        }

        [Fact]
        public async Task be_active_when_claim_type_and_value_are_successfully_configured()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("ClaimValues", "some_claim_value")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var claimValueToggle = new ClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await claimValueToggle.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_active_when_claim_type_and_value_are_successfully_configured_and_user_have_many_claim_of_the_same_type()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("ClaimValues", "three")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] 
                { 
                    new Claim("some_claim_type", "one"),
                    new Claim("some_claim_type", "two"),
                    new Claim("some_claim_type", "three")
                }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var claimValueToggle = new ClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await claimValueToggle.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeTrue();
        }


        [Fact]
        public async Task be_not_active_when_claim_type_and_value_are_successfully_configured()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("ClaimValues", "some_claim_value")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "not_some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var claimValueToggle = new ClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await claimValueToggle.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeFalse();
        }
        private class FakeHttpContextAccesor
            : IHttpContextAccessor
        {
            public HttpContext HttpContext { get; set; }

            public FakeHttpContextAccesor() { }

            public FakeHttpContextAccesor(HttpContext context)
            {
                HttpContext = context;
            }
        }
    }
}
