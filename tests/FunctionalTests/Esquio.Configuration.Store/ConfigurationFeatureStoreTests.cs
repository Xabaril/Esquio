using Esquio.Abstractions;
using FluentAssertions;
using System.Threading;
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
        public async Task return_null_when_find_a_non_existing_feature()
        {
            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", "non-valid-product-name","ring"))
                .Should().BeNull();

            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", Product,"ring"))
                .Should().BeNull();
        }

        [Fact]
        public async Task return_feature_if_is_configured()
        {
            (await _fixture.FeatureStore.FindFeatureAsync("non-valid-feature-name", "non-valid-application-name", "ring"))
                .Should().BeNull();

            var feature = await _fixture.FeatureStore.FindFeatureAsync("FeatureA", Product, "ring");

            feature.Should().NotBeNull();

            feature.Name.Should().BeEquivalentTo("FeatureA");
            feature.IsEnabled.Should().BeTrue();
        }
    }

    class FakeToggle 
        : IToggle
    {
        public Task<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}
