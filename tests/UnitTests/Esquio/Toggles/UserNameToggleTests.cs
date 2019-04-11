using Esquio;
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
    public class UserNameToggleShould
    {
        [Fact]
        public async Task be_not_active_if_user_provider_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
                .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user1;user2"))
                .Build();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await new UserNameToggle().IsActiveAsync(featureContext);
            });
        }
        [Fact]
        public async Task be_not_active_if_user_is_null()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user2"))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => null))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_parameter_is_not_configured()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => null))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_not_active_if_user_is_not_contained_on_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
              .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user2"))
              .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
              .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeFalse();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_to_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user1"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_equal_non_sensitive_to_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user1"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "UsEr1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }
        [Fact]
        public async Task be_active_if_user_is_contained_on_users_parameters_value()
        {
            var featureContext = Builders.FeatureContextBuilder()
               .WithService<IFeatureStore, DelegatedFeatureStore>(new DelegatedFeatureStore(() => "user1;user2"))
               .WithService<IUserNameProviderService, DelegatedUserNameProviderService>(new DelegatedUserNameProviderService(() => "user1"))
               .Build();

            var active = await new UserNameToggle().IsActiveAsync(featureContext);
            active.Should().BeTrue();
        }

        class DelegatedFeatureStore
            : IFeatureStore
        {
            Func<string> _getConfiguredUsersFunc;

            public DelegatedFeatureStore(Func<string> getConfiguredUsersFunc)
            {
                _getConfiguredUsersFunc = getConfiguredUsersFunc ?? throw new ArgumentNullException(nameof(getConfiguredUsersFunc));
            }
            public Task<object> GetParameterValueAsync<TToggle>(string application, string feature, string parameterName) where TToggle : IToggle
            {
                var configuredUsers = _getConfiguredUsersFunc();
                return Task.FromResult((object)configuredUsers);
            }
            public Task<bool> AddFeatureAsync(string application, string feature, bool enabled = false)
            {
                return Task.FromResult(true);
            }
            public Task<bool> AddToggleAsync<TToggle>(string application, string feature, IDictionary<string, object> parameterValues) where TToggle : IToggle
            {
                return Task.FromResult(true);
            }

            public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Type>> FindTogglesTypesAsync(string applicationName, string featureName)
            {
                throw new NotImplementedException();
            }
        }
        class DelegatedUserNameProviderService
            : IUserNameProviderService
        {
            Func<string> _getCurrentUserFunc;

            public DelegatedUserNameProviderService(Func<string> getCurrentUserFunc)
            {
                _getCurrentUserFunc = getCurrentUserFunc ?? throw new ArgumentNullException(nameof(getCurrentUserFunc));
            }
            public Task<string> GetCurrentUserNameAsync()
            {
                return Task.FromResult(_getCurrentUserFunc());
            }
        }
    }
}
