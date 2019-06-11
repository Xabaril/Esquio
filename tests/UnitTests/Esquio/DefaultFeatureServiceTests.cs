using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Model;
using Esquio.Toggles;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.Esquio
{
    public class defaultfeatureservice_should
    {
        [Fact]
        public async Task throw_if_feature_service_throw_and_exceptionbehavior_is_throw()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(ThrowInvalidOperationExceptionToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.Throw);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await featureService.IsEnabledAsync("sample");
            });
        }

        [Fact]
        public async Task be_disabled_if_feature_service_throw_and_exceptionbehavior_is_setasnotactive()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(ThrowInvalidOperationExceptionToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetAsNotActive);

            var active = await featureService.IsEnabledAsync("sample");

            active.Should()
                .BeFalse();
        }

        [Fact]
        public async Task be_enabled_if_feature_service_throw_and_exceptionbehavior_is_setasactive()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(ThrowInvalidOperationExceptionToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetAsActive);

            var active = await featureService.IsEnabledAsync("sample");

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_enabled_when_feature_exist_and_toggles_are_active()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(OnToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetAsActive);

            var active = await featureService.IsEnabledAsync("sample");

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_enabled_when_feature_exist_but_any_toggle_is_not_active()
        {
            var feature = Build.Feature("sample")
               .Enabled()
               .AddOne(new Toggle(typeof(OffToggle).FullName))
               .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetAsActive);

            var active = await featureService.IsEnabledAsync("sample");

            active.Should()
                .BeFalse();
        }
        [Fact]
        public async Task be_enabled_when_feature_not_exist_and_notfound_behavioris_setasactive()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(OnToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, notFoundBehavior: NotFoundBehavior.SetAsActive);

            var active = await featureService.IsEnabledAsync("sample");

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_disabled_when_feature_not_exist_and_notfound_behavioris_setasnotactive()
        {
            var feature = Build.Feature("sample")
               .Enabled()
               .AddOne(new Toggle(typeof(OffToggle).FullName))
               .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, notFoundBehavior: NotFoundBehavior.SetAsNotActive);

            var active = await featureService.IsEnabledAsync("non-existing-feature");

            active.Should()
                .BeFalse();
        }

        private IFeatureService CreateFeatureService(List<Feature> configuredFeatures, OnErrorBehavior onErrorBehavior = OnErrorBehavior.SetAsNotActive, NotFoundBehavior notFoundBehavior = NotFoundBehavior.SetAsNotActive)
        {
            var store = new FakeRuntimeStore(configuredFeatures);
            var activator = new FakeToggleActivator();

            var esquioOptions = new EsquioOptions();
            esquioOptions.ConfigureOnErrorBehavior(onErrorBehavior);
            esquioOptions.ConfigureNotFoundBehavior(notFoundBehavior);

            var options = Options.Create<EsquioOptions>(esquioOptions);
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<DefaultFeatureService>();

            return new DefaultFeatureService(store, activator, options, logger);
        }
        private class FakeRuntimeStore
            : IRuntimeFeatureStore
        {
            List<Feature> _defaultFeatures;
            public FakeRuntimeStore(List<Feature> defaultFeatures)
            {
                _defaultFeatures = defaultFeatures ?? throw new ArgumentNullException(nameof(defaultFeatures));
            }
            public Task<Feature> FindFeatureAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_defaultFeatures.Where(f => f.Name == featureName)
                    .SingleOrDefault());
            }
        }

        private class ThrowInvalidOperationExceptionToggle
            : IToggle
        {
            public Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
            {
                throw new InvalidOperationException("throw exception");
            }
        }

        private class FakeToggleActivator
            : IToggleTypeActivator
        {
            public IToggle CreateInstance(string toggleTypeName)
            {
                return (IToggle)Activator.CreateInstance(_toggleTypes[toggleTypeName]);
            }

            private Dictionary<string, Type> _toggleTypes = new Dictionary<string, Type>()
            {
                {typeof(OnToggle).FullName,typeof(OnToggle) },
                {typeof(OffToggle).FullName,typeof(OffToggle) },
                {typeof(ThrowInvalidOperationExceptionToggle).FullName,typeof(ThrowInvalidOperationExceptionToggle) }
            };
        }
    }
}
