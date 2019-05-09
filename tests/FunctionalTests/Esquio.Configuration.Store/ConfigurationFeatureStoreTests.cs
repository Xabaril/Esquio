using Esquio;
using Esquio.Abstractions;
using Esquio.Model;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.Configuration.Store
{
    public class ConfigurationFeatureStoreShould : IClassFixture<Fixture>
    {
        private const string Product = "MySampleProduct";
        private readonly Fixture _fixture;

        public ConfigurationFeatureStoreShould(Fixture fixture)
        {
            _fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void return_false_when_try_to_add_new_feature()
        {
            Action act = () => _fixture.FeatureStore.AddFeatureAsync("product", new Feature("featureTest")).RunSynchronously();

            act.Should().Throw<EsquioException>();
        }
        [Fact]
        public async Task return_null_when_find_a_non_existing_feature()
        {
            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", "non-valid-application-name"))
                .Should().BeNull();

            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", Product))
                .Should().BeNull();
        }
        [Fact]
        public async Task return_feature_if_is_configured()
        {
            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", "non-valid-application-name"))
                .Should().BeNull();

            var feature = await _fixture.FeatureStore.FindFeatureAsync("FeatureA", Product);

            feature.Should().NotBeNull();

            feature.Name.Should().BeEquivalentTo("FeatureA");
            feature.IsEnabled.Should().BeTrue();
        }
    }

    class FakeToggle : IToggle
    {
        public Task<bool> IsActiveAsync(string applicationName, string featureName)
        {
            return Task.FromResult(false);
        }
    }
}
