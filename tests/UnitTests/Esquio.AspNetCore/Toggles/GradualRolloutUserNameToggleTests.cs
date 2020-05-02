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
    public class gradualrollout_toggle_should
    {
        private const string Percentage = nameof(Percentage);

        [Fact]
        public void throw_if_partitioner_is_null()
        {
            var toggle = Build
                .Toggle<GradualRolloutUserNameToggle>()
                .AddOneParameter(Percentage, 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "User1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new GradualRolloutUserNameToggle(partitioner: null, contextAccessor);
            });
        }

        [Fact]
        public void throw_if_username_provider_is_null()
        {
            var toggle = Build
                .Toggle<GradualRolloutUserNameToggle>()
                .AddOneParameter(Percentage, 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var partitioner = new DefaultValuePartitioner();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new GradualRolloutUserNameToggle(partitioner, httpContextAccessor: null);
            });
        }

        [Fact]
        public async Task be_active_when_percentage_is_hundred_percent()
        {
            var toggle = Build
                .Toggle<GradualRolloutUserNameToggle>()
                .AddOneParameter(Percentage, 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "User1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var partitioner = new DefaultValuePartitioner();

            var active = await new GradualRolloutUserNameToggle(partitioner, contextAccessor).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_no_active_when_percentage_is_zero_percent()
        {
            var toggle = Build
                   .Toggle<GradualRolloutUserNameToggle>()
                   .AddOneParameter(Percentage, 0)
                   .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, "User1") }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var partitioner = new DefaultValuePartitioner();

            var active = await new GradualRolloutUserNameToggle(partitioner, contextAccessor).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeFalse();
        }

        [Theory]
        [InlineData("sample1", 40)]
        [InlineData("sample1", 10)]
        [InlineData("sample1", 70)]
        [InlineData("sample2", 70)]
        [InlineData("sample3", 30)]
        [InlineData("sample4", 80)]
        [InlineData("sample5", 100)]
        [InlineData("sample6", 10)]
        [InlineData("sample6", 30)]
        public async Task be_active_when_user_partition_is_on_percent_bucket(string username, int percentage)
        {
            var partition = new DefaultValuePartitioner().ResolvePartition(Constants.FeatureName + username,partitions:100);
            var expected = partition <= percentage;

            var toggle = Build
                   .Toggle<GradualRolloutUserNameToggle>()
                   .AddOneParameter(Percentage, percentage)
                   .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimsIdentity.DefaultNameClaimType, username) }, "cookies"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var partitioner = new DefaultValuePartitioner();

            var active = await new GradualRolloutUserNameToggle(partitioner, contextAccessor).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().Be(expected);
        }
    }
}
