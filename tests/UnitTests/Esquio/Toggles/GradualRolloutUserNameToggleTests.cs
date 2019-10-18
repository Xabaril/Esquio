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

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            Assert.Throws<ArgumentNullException>(() =>
            {
                new GradualRolloutUserNameToggle(partitioner: null, userNameProvider, store);
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

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var partitioner = new DefaultValuePartitioner();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new GradualRolloutUserNameToggle(partitioner, userNameProviderService: null, store);
            });
        }

        [Fact]
        public void throw_if_runtimestore_provider_is_null()
        {
            var toggle = Build
                   .Toggle<GradualRolloutUserNameToggle>()
                   .AddOneParameter(Percentage, 100)
                   .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();


            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");
            var partitioner = new DefaultValuePartitioner();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new GradualRolloutUserNameToggle(partitioner, userNameProvider, featureStore: null);
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
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");
            var partitioner = new DefaultValuePartitioner();

            var active = await new GradualRolloutUserNameToggle(partitioner, userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

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
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");
            var partitioner = new DefaultValuePartitioner();

            var active = await new GradualRolloutUserNameToggle(partitioner, userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

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

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => username);
            var partitioner = new DefaultValuePartitioner();

            var actual = await new GradualRolloutUserNameToggle(partitioner, userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            actual.Should().Be(expected);
        }
    }
}
