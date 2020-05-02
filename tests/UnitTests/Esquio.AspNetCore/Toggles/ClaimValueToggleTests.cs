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
    public class claimvalue_toggle_tests
    {
        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var store = new DelegatedValueFeatureStore((_, __, ___) => null);
                new ClaimValueToggle(null);
            });
        }

        [Fact]
        public async Task be_active_when_claim_type_and_value_are_successfully_configured()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddParameter("ClaimType", "some_claim_type")
                .AddParameter("ClaimValues", "some_claim_value")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __, ___) => feature);
            var claimValueToggle = new ClaimValueToggle(new FakeHttpContextAccessor(context));

            var active = await claimValueToggle.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_not_active_when_claim_type_and_value_are_successfully_configured()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddParameter("ClaimType", "some_claim_type")
                .AddParameter("ClaimValues", "some_claim_value")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "not_some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __, ___) => feature);
            var claimValueToggle = new ClaimValueToggle(new FakeHttpContextAccessor(context));

            var active = await claimValueToggle.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeFalse();
        }

        [Fact]
        public async Task be_not_active_when_claim_type_and_value_are_successfully_configured_and_user_claims_contains_multiple_values_of_the_sample_type()
        {
            var toggle = Build
                .Toggle<ClaimValueToggle>()
                .AddParameter("ClaimType", "some_claim_type")
                .AddParameter("ClaimValues", "three")
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

            var store = new DelegatedValueFeatureStore((_, __, ___) => feature);
            var claimValueToggle = new ClaimValueToggle(new FakeHttpContextAccessor(context));

            var active = await claimValueToggle.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeFalse();
        }

    }
}
