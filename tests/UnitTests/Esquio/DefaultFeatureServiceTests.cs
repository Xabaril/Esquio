using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Diagnostics;
using Esquio.Model;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
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
        public async Task be_disabled_if_feature_service_throw_and_exceptionbehavior_is_setasdisabled()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(ThrowInvalidOperationExceptionToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetDisabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeFalse();
        }

        [Fact]
        public async Task be_enabled_if_feature_service_throw_and_exceptionbehavior_is_setasenabled()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(ThrowInvalidOperationExceptionToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_enabled_when_feature_exist_and_toggles_are_active()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(AllwaysOnToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeTrue();
        }
        [Fact]
        public async Task use_session_if_feature_exist_before_evaluate()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(AllwaysOnToggle).FullName))
                .Build();

            var session = new PersistEvaluationSession();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, evaluationSession: session, onErrorBehavior: OnErrorBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeTrue();

            feature.Disabled();

            enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_disabled_when_feature_exist_but_any_toggle_is_not_active()
        {
            var feature = Build.Feature("sample")
               .Enabled()
               .AddOne(new Toggle(typeof(AllwaysOffToggle).FullName))
               .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, onErrorBehavior: OnErrorBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeFalse();
        }
        [Fact]
        public async Task be_enabled_when_feature_not_exist_and_notfound_behavior_is_setenabled()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle(typeof(AllwaysOnToggle).FullName))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, notFoundBehavior: NotFoundBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("non_existing-feature");

            enabled.Should()
                .BeTrue();
        }
        [Fact]
        public async Task be_disabled_when_feature_exist_but_toggle_type_can_be_created()
        {
            var feature = Build.Feature("sample")
                .Enabled()
                .AddOne(new Toggle("Non_Existing_Toggle_Type"))
                .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, notFoundBehavior: NotFoundBehavior.SetEnabled);

            var enabled = await featureService.IsEnabledAsync("sample");

            enabled.Should()
                .BeFalse();
        }

        [Fact]
        public async Task be_disabled_when_feature_not_exist_and_notfound_behavioris_setasdisabled()
        {
            var feature = Build.Feature("sample")
               .Enabled()
               .AddOne(new Toggle(typeof(AllwaysOffToggle).FullName))
               .Build();

            var featureService = CreateFeatureService(new List<Feature>() { feature }, notFoundBehavior: NotFoundBehavior.SetDisabled);

            var enabled = await featureService.IsEnabledAsync("non-existing-feature");

            enabled.Should()
                .BeFalse();
        }

        private IFeatureService CreateFeatureService(List<Feature> configuredFeatures, IScopedEvaluationHolder evaluationSession = null, OnErrorBehavior onErrorBehavior = OnErrorBehavior.SetDisabled, NotFoundBehavior notFoundBehavior = NotFoundBehavior.SetDisabled)
        {
            var store = new FakeRuntimeStore(configuredFeatures);
            var activator = new FakeToggleActivator();
            var session = evaluationSession ?? new NoScopedEvaluationHolder();

            var esquioOptions = new EsquioOptions();
            esquioOptions.ConfigureOnErrorBehavior(onErrorBehavior);
            esquioOptions.ConfigureNotFoundBehavior(notFoundBehavior);

            var options = Options.Create<EsquioOptions>(esquioOptions);
            var loggerFactory = new LoggerFactory();

            var esquioDiagnostics = new EsquioDiagnostics(loggerFactory);

            return new DefaultFeatureService(store, activator, session, options, esquioDiagnostics);
        }
        private class FakeRuntimeStore
            : IRuntimeFeatureStore
        {
            List<Feature> _defaultFeatures;
            public FakeRuntimeStore(List<Feature> defaultFeatures)
            {
                _defaultFeatures = defaultFeatures ?? throw new ArgumentNullException(nameof(defaultFeatures));
            }
            public Task<Feature> FindFeatureAsync(string featureName, string productName, string deploymentName, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_defaultFeatures.Where(f => f.Name == featureName)
                    .SingleOrDefault());
            }
        }

        private class ThrowInvalidOperationExceptionToggle
            : IToggle
        {
            public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
            {
                throw new InvalidOperationException("throw exception");
            }
        }

        private class FakeToggleActivator
            : IToggleTypeActivator
        {
            public IToggle CreateInstance(string toggleTypeName)
            {
                if (_toggleTypes.ContainsKey(toggleTypeName))
                {
                    return (IToggle)Activator.CreateInstance(_toggleTypes[toggleTypeName]);
                }

                return null;
            }

            private Dictionary<string, Type> _toggleTypes = new Dictionary<string, Type>()
            {
                {typeof(AllwaysOnToggle).FullName,typeof(AllwaysOnToggle) },
                {typeof(AllwaysOffToggle).FullName,typeof(AllwaysOffToggle) },
                {typeof(ThrowInvalidOperationExceptionToggle).FullName,typeof(ThrowInvalidOperationExceptionToggle) }
            };
        }

        private class PersistEvaluationSession
            : IScopedEvaluationHolder
        {
            Dictionary<string, bool> _evaluationResults = new Dictionary<string, bool>();

            public Task SetAsync(string featureName, bool enabled)
            {
                if (!_evaluationResults.ContainsKey(featureName))
                {
                    _evaluationResults.Add(featureName, enabled);
                }

                return Task.CompletedTask;
            }

            public Task<bool> TryGetAsync(string featureName, out bool enabled)
            {
                enabled = _evaluationResults.ContainsKey(featureName)
                    &&
                    _evaluationResults[featureName];

                return Task.FromResult(enabled);
            }
        }
    }
}
