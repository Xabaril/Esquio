using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using UnitTests.Seedwork.Builders;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class FromToToggleShould
    {
        [Fact]
        public async Task throw_if_store_service_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder().Build();

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await new FromToToggle(null).IsActiveAsync(featureContext);
            });
        }
        [Fact]
        public async Task be_not_active_if_now_is_not_between_configured_dates()
        {
            var featureContext = Builders.FeatureContextBuilder().Build();
            var store = new DelegatedValueFeatureStore(param =>
              {
                  if (param == "From") return DateTime.UtcNow.AddMonths(-1).ToString(FromToToggle.FORMAT_DATE);
                  else if (param == "To") return DateTime.UtcNow.AddMonths(-1).AddDays(10).ToString(FromToToggle.FORMAT_DATE);
                  else
                      throw new InvalidOperationException("Parameter not know!");

              });

            var active = await new FromToToggle(store).IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_now_is_between_configured_dates()
        {
            var featureContext = Builders.FeatureContextBuilder().Build();
            var store = new DelegatedValueFeatureStore(param =>
              {
                  if (param == "From") return DateTime.UtcNow.AddMonths(-1).ToString(FromToToggle.FORMAT_DATE);
                  else if (param == "To") return DateTime.UtcNow.AddMonths(-1).AddDays(10).ToString(FromToToggle.FORMAT_DATE);
                  else
                      throw new InvalidOperationException("Parameter not know!");

              });

            var active = await new FromToToggle(store).IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
    }
}
