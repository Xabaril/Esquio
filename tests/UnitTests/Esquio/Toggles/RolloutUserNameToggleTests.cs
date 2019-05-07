using Esquio.Abstractions;
using Esquio.Toggles;
using FluentAssertions;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class RolloutUserNameToggleShould
    {
        private const string Percentage = nameof(Percentage);

        [Fact]
        public async Task be_active_when_percentage_is_hundred_percent()
        {
            var toggle = Build
                   .Toggle<RolloutUserNameToggle>()
                   .AddOneParameter(Percentage, 100)
                   .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_no_active_when_percentage_is_zero_percent()
        {
            var toggle = Build
                   .Toggle<RolloutUserNameToggle>()
                   .AddOneParameter(Percentage, 0)
                   .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => "User1");

            var active = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            active.Should().BeFalse();
        }
        [Theory]
        [InlineData("sample1",40)]
        [InlineData("sample1", 10)]
        [InlineData("sample1", 70)]
        [InlineData("sample2",70)]
        [InlineData("sample3",30)]
        [InlineData("sample4",80)]
        [InlineData("sample5",100)]
        [InlineData("sample6",10)]
        [InlineData("sample6", 30)]
        public async Task be_active_when_user_partition_is_on_percent_bucket(string username,int percentage)
        {
            var partition = Partitioner.ResolveToLogicalPartition(username, 10);
            var expected = partition <= ((10 * percentage) / 100);
            var toggle = Build
                   .Toggle<RolloutUserNameToggle>()
                   .AddOneParameter(Percentage, percentage)
                   .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var userNameProvider = new DelegatedUserNameProviderService(() => username);

            var actual = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.FeatureName, Constants.ApplicationName);

            actual.Should().Be(expected);
        }
    }
}
