using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class FromToToggleShould
    {
        private const string From = nameof(From);
        private const string To = nameof(To);

        [Fact]
        public async Task throw_if_store_service_is_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new FromToToggle(null).IsActiveAsync(Constants.FeatureName, Constants.ProductName);
            });
        }
        [Fact]
        public async Task be_not_active_if_now_is_not_between_configured_dates()
        {
            var toggle = Build
                .Toggle<FromToToggle>()
                .AddOneParameter(From, DateTime.UtcNow.AddMonths(-1).ToString(FromToToggle.FORMAT_DATE))
                .AddOneParameter(To, DateTime.UtcNow.AddMonths(-1).AddDays(10).ToString(FromToToggle.FORMAT_DATE))
                .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new FromToToggle(store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_now_is_between_configured_dates()
        {
            var toggle = Build
                   .Toggle<FromToToggle>()
                   .AddOneParameter(From, DateTime.UtcNow.AddMonths(-1).ToString(FromToToggle.FORMAT_DATE))
                   .AddOneParameter(To, DateTime.UtcNow.AddMonths(1).ToString(FromToToggle.FORMAT_DATE))
                   .Build();
            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            var store = new DelegatedValueFeatureStore((_, __) => feature);

            var active = await new FromToToggle(store).IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeTrue();
        }
    }
}
