using Esquio.Abstractions;
using Esquio.Toggles;
using FluentAssertions;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class RolloutUserNameToggleShould
    {
        [Fact]
        public async Task be_active_when_percentage_is_hundred_percent()
        {
            var store = new DelegatedValueFeatureStore(_ =>
            {
                return 100;
            });
            var userNameProvider = new DelegatedUserNameProviderService(() =>
            {
                return "User1";
            });

            var active = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);

            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_no_active_when_percentage_is_zero_percent()
        {
            var store = new DelegatedValueFeatureStore(_ =>
            {
                return 0;
            });
            var userNameProvider = new DelegatedUserNameProviderService(() =>
            {
                return "User1";
            });

            var active = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);

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
            var store = new DelegatedValueFeatureStore(_ =>
            {
                return percentage;
            });
            var userNameProvider = new DelegatedUserNameProviderService(() =>
            {
                return username;
            });

            var actual = await new RolloutUserNameToggle(userNameProvider, store).IsActiveAsync(Constants.ApplicationName, Constants.FeatureName);
            var expected = partition <= ((10 * percentage) / 100);

            actual.Should().Be(expected);
        }
    }
}
