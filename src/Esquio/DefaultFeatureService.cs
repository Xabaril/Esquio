using Esquio.Abstractions;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio
{
    public sealed class DefaultFeatureService
        : IFeatureService
    {
        private readonly IFeatureStore _featureStore;
        private readonly IToggleTypeActivator _toggleActivator;
        private readonly IFeatureContextFactory _featureContextFactory;
        private readonly ILogger<DefaultFeatureService> _logger;
        public DefaultFeatureService(IFeatureStore store, IToggleTypeActivator toggeActivator, IFeatureContextFactory featureContextFactory, ILogger<DefaultFeatureService> logger)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggeActivator ?? throw new ArgumentNullException(nameof(toggeActivator));
            _featureContextFactory = featureContextFactory ?? throw new ArgumentNullException(nameof(featureContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> IsEnabledAsync(string applicationName, string featureName)
        {
            try
            {
                Log.FeatureServiceProcessingBegin(_logger, featureName, applicationName);

                var feature = await _featureStore.FindFeatureAsync(applicationName, featureName);

                if (feature != null)
                {
                    var togglesTypes = await _featureStore.FindTogglesTypesAsync(applicationName, featureName);
                    var context = _featureContextFactory.Create(applicationName, featureName);

                    foreach (var type in togglesTypes)
                    {
                        var toggle = _toggleActivator.CreateInstance(type);

                        //TODO: Issues on create instances, user defined fallback behavior!!!!!

                        if (!await toggle.IsActiveAsync(context))
                        {
                            Log.FeatureServiceToggleIsNotActive(_logger, featureName, applicationName);
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    Log.FeatureServiceNotFoundFeature(_logger, featureName, applicationName);
                    return false;
                }
            }
            catch (Exception exception)
            {
                Log.FeatureServiceProcessingFail(_logger, featureName, applicationName, exception);
                return false;
            }
        }
        private static class Log
        {
            public static void FeatureServiceProcessingBegin(ILogger logger, string featureName, string applicationName)
            {
                _featureServiceBegin(logger, featureName, applicationName, null);
            }
            public static void FeatureServiceNotFoundFeature(ILogger logger, string featureName, string applicationName)
            {
                _featureServiceNotFound(logger, featureName, applicationName, null);
            }
            public static void FeatureServiceToggleIsNotActive(ILogger logger, string featureName, string applicationName)
            {
                _featureServiceNotActive(logger, featureName, applicationName, null);
            }
            public static void FeatureServiceProcessingFail(ILogger logger, string featureName, string applicationName, Exception exception)
            {
                _featureServiceThrow(logger, featureName, applicationName, exception);
            }

            private static readonly Action<ILogger, string, string, Exception> _featureServiceBegin = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                EventIds.DefaultFeatureServiceBegin,
                "Running DefaultFeatureService to check {featureName} for {applicationName}");

            private static readonly Action<ILogger, string, string, Exception> _featureServiceNotFound = LoggerMessage.Define<string, string>(
                LogLevel.Warning,
                EventIds.FeatureNotFound,
                "The feature {feature} is not configured in the store for application {application}.");

            private static readonly Action<ILogger, string, string, Exception> _featureServiceNotActive = LoggerMessage.Define<string, string>(
               LogLevel.Debug,
               EventIds.FeatureNotActive,
               "The feature {feature} is not active on application {application}.");

            private static readonly Action<ILogger, string, string, Exception> _featureServiceThrow = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                EventIds.DefaultFeatureServiceThrows,
                "DefaultFeatureService threw an unhandled exception checking {featureName} for application {application}");
        }
    }
}
