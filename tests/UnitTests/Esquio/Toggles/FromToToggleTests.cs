using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Seedwork.Builders;
using Xunit;
namespace UnitTests.Esquio.Toggles
{
    public class FromToToggleShould
    {
        [Fact]
        public async Task throw_if_store_service_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
                .Build();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await new FromToToggle().IsActiveAsync(featureContext);
            });
        }
        [Fact]
        public async Task be_not_active_if_now_is_not_between_configured_dates()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => DateTime.UtcNow.AddMonths(-1)))
              .Build();
            //from is addMonth(-1) and to is addMonth(-1) + 10 days 

            var active = await new FromToToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_now_is_between_configured_dates()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => DateTime.UtcNow.AddDays(-1)))
              .Build();
            //from is addDays(-1) and to is addDays(-1) + 10 days 

            var active = await new FromToToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        class DelegatedFeatureStore
            : IFeatureStore
        {
            Func<DateTime> _getConfiguredFromDateFunc;

            public DelegatedFeatureStore(Func<DateTime> getConfiguredFromDateFunc)
            {
                _getConfiguredFromDateFunc = getConfiguredFromDateFunc ?? throw new ArgumentNullException(nameof(getConfiguredFromDateFunc));
            }
            public Task<object> GetParameterValueAsync<TToggle>(string application, string feature, string parameterName) where TToggle : IToggle
            {
                var fromDate = _getConfiguredFromDateFunc()
                    .ToString(FromToToggle.FORMAT_DATE);

                var toDate = _getConfiguredFromDateFunc()
                    .AddDays(10)
                    .ToString(FromToToggle.FORMAT_DATE);

                if (parameterName.Equals("From"))
                {
                    return Task.FromResult((object)fromDate);
                }
                else
                {
                    return Task.FromResult((object)toDate);
                }
            }
            public Task<bool> AddFeatureAsync(string application, string feature, bool enabled = false)
            {
                return Task.FromResult(true);
            }
            public Task<bool> AddToggleAsync<TToggle>(string application, string feature, IDictionary<string, object> parameterValues) where TToggle : IToggle
            {
                return Task.FromResult(true);
            }
        }
        class DelegatedRoleNameProviderService
            : IRoleNameProviderService
        {
            Func<string> _getCurrentRoleFunc;
            public DelegatedRoleNameProviderService(Func<string> getCurrentRoleFunc)
            {
                _getCurrentRoleFunc = getCurrentRoleFunc ?? throw new ArgumentNullException(nameof(getCurrentRoleFunc));
            }
            public Task<string> GetCurrentRoleNameAsync()
            {
                return Task.FromResult(_getCurrentRoleFunc());
            }
        }
    }
}
