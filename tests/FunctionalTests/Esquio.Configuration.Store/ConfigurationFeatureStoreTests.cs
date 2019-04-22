using Esquio.Abstractions;
using Esquio.Configuration.Store;
using Esquio.Configuration.Store.Configuration;
using Esquio.Toggles;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.Configuration.Store
{
    public class ConfigurationFeatureStoreShould
    {

        private readonly ConfigurationFeatureStore _featureStore;

        public ConfigurationFeatureStoreShould()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Esquio.Configuration.Store"))
               .AddJsonFile("appsettings.json")
               .Build();

            var esquio = new EsquioConfiguration();
            configuration.Bind("Esquio", esquio);

            var options = Options.Create(esquio);
            var logger = new LoggerFactory().CreateLogger<ConfigurationFeatureStore>();

            _featureStore = new ConfigurationFeatureStore(options, logger);
        }
        [Fact]
        public async Task return_false_when_try_to_add_new_feature()
        {
            (await _featureStore.AddFeatureAsync("applicationName", "featureName", enabled: true))
                .Should().BeFalse();
        }
        [Fact]
        public async Task return_null_when_find_a_non_existing_feature()
        {
            (await _featureStore.FindFeatureAsync("non-valid-application-name","non-valid-feature-name"))
                .Should().BeNull();

            (await _featureStore.FindFeatureAsync("MySampleApplication", "non-valid-feature-name"))
                .Should().BeNull();
        }
        [Fact]
        public async Task return_feature_if_is_configured()
        {
            (await _featureStore.FindFeatureAsync("non-valid-application-name", "non-valid-feature-name"))
                .Should().BeNull();

            var feature = await _featureStore.FindFeatureAsync("MySampleApplication", "FeatureA");

            feature.Should().NotBeNull();

            feature.Name.Should().BeEquivalentTo("FeatureA");
            feature.Enabled.Should().BeTrue();
        }
        [Fact]
        public async Task return_false_when_try_to_add_new_toggle()
        {
            (await _featureStore.AddToggleAsync<FakeToggle>("applicationName", "featureName",new Dictionary<string,object>()))
               .Should().BeFalse();
        }
        [Fact]
        public async Task return_empty_collection_when_find_toggle_types_on_non_existing_feature()
        {
            (await _featureStore.FindTogglesTypesAsync("applicationName", "featureName"))
               .Any().Should().BeFalse();
        }
        [Fact]
        public async Task return_configured_toggle_types()
        {
            var configuredTypes = await _featureStore.FindTogglesTypesAsync("MySampleApplication", "FeatureA");

            configuredTypes.Count().Should().Be(2);

            configuredTypes.Contains("Esquio.Toggles.OnToggle").Should().BeTrue();
            configuredTypes.Contains("Esquio.Toggles.UserNameToggle").Should().BeTrue();
        }
        [Fact]
        public async Task return_null_when_find_parameter_value_on_non_existing_feature()
        {
            (await _featureStore.GetToggleParameterValueAsync<UserNameToggle>("MySampleApplication", "FeatureB","Users"))
                .Should().BeNull();
        }
        [Fact]
        public async Task return_null_when_find_parameter_value_on_non_existing_toggle()
        {
            (await _featureStore.GetToggleParameterValueAsync<OffToggle>("MySampleApplication", "FeatureA", "Users"))
                .Should().BeNull();
        }
        [Fact]
        public async Task return_parameter_value_on_existing_feature()
        {
            var parameterValue = await _featureStore.GetToggleParameterValueAsync<UserNameToggle>("MySampleApplication", "FeatureA", "Users");

            parameterValue.Should().NotBeNull();
            parameterValue.Should().BeEquivalentTo("user1;user2;user3");
        }
    }

    class FakeToggle : IToggle
    {
        public Task<bool> IsActiveAsync(IFeatureContext context)
        {
            return Task.FromResult(false);
        }
    }
}
