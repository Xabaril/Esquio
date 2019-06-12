using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class gradualrolloutclaimvalue_should
    {
        [Fact]
        public void throw_if_store_service_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var accessor = new FakeHttpContextAccesor();
                new GradualRolloutClaimValueToggle(null, accessor);
            });
        }

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
           {
               var store = new DelegatedValueFeatureStore((_, __) => null);
               new GradualRolloutClaimValueToggle(store, null);
           });
        }

        [Fact]
        public async Task be_active_when_claim_value_is_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("Percentage", 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var gradualRolloutClaimValue = new GradualRolloutClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutClaimValue.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeTrue();
        }
        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(70)]
        [InlineData(72)]
        [InlineData(30)]
        [InlineData(80)]
        [InlineData(100)]
        [InlineData(11)]
        [InlineData(33)]
        public async Task use_partition_for_claim_value(int percentage)
        {
            var valid = false;
            var claim_value = default(string);
            do
            {
                claim_value = Guid.NewGuid().ToString();
                var partition = global::Esquio.Abstractions.Partitioner.ResolveToLogicalPartition(claim_value, 100);

                if (partition <= percentage)
                {
                    valid = true;
                }

            } while (!valid);

            var toggle = Build
                .Toggle<GradualRolloutClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("Percentage", percentage)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", claim_value) }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var gradualRolloutClaimValue = new GradualRolloutClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutClaimValue.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_non_active_when_claim_value_is_not_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutClaimValueToggle>()
                .AddOneParameter("ClaimType", "some_claim_type")
                .AddOneParameter("Percentage", 0)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim("some_claim_type", "some_claim_value") }, "cookies"));

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var gradualRolloutClaimValue = new GradualRolloutClaimValueToggle(store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutClaimValue.IsActiveAsync(Constants.FeatureName);

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
