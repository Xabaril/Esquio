using Esquio.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Esquio.Extensions
{
    public class servicecollection_extensions_should
    {
        [Fact]
        public void addif_register_service_using_enabled_factory_when_feature_is_enabled()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            var featureService = new FakeFeatureService(enabled: true);

            serviceCollection.AddTransient<IFeatureService>(sp =>
            {
                return featureService;
            });

            serviceCollection.Add<IAddIfService>("feature",
                sp =>
                {
                    return new AddIfServiceEnabled();
                },
                sp =>
                {
                    return new AddIfServiceDisabled();
                }, ServiceLifetime.Transient);

            var sp = serviceCollection.BuildServiceProvider();

            sp.GetRequiredService<IAddIfService>()
                .GetType()
                .Should().Be(typeof(AddIfServiceEnabled));
        }

        [Fact]
        public void addif_register_service_using_disabled_factory_when_feature_is_not_enabled()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            var featureService = new FakeFeatureService(enabled: false);

            serviceCollection.AddTransient<IFeatureService>(sp =>
            {
                return featureService;
            });

            serviceCollection.Add<IAddIfService>("feature",
                sp =>
                {
                    return new AddIfServiceEnabled();
                },
                sp =>
                {
                    return new AddIfServiceDisabled();
                }, ServiceLifetime.Transient);

            var sp = serviceCollection.BuildServiceProvider();

            sp.GetRequiredService<IAddIfService>()
                .GetType()
                .Should().Be(typeof(AddIfServiceDisabled));
        }

        class FakeFeatureService : IFeatureService
        {
            bool _enabled;

            public FakeFeatureService(bool enabled)
            {
                _enabled = enabled;
            }
            public Task<bool> IsEnabledAsync(string featureName, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_enabled);
            }
        }

        interface IAddIfService
        {
        }

        class AddIfServiceEnabled: IAddIfService
        {
        }

        class AddIfServiceDisabled : IAddIfService
        {
        }
    }
}
