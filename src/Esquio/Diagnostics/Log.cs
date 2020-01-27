using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Diagnostics
{
    static class Log
    {
        public static void FeatureServiceProcessingBegin(ILogger logger, string featureName, string productName, string ringName)
        {
            _featureServiceBegin(logger, featureName, productName, ringName, null);
        }
        public static void FeatureServiceFromSession(ILogger logger, string featureName, string productName, string ringName)
        {
            _featureServiceFromSession(logger, featureName, productName, ringName, null);
        }
        public static void FeatureServiceNotFoundFeature(ILogger logger, string featureName, string productName, string ringName)
        {
            _featureServiceNotFound(logger, featureName, productName, ringName, null);
        }
        public static void FeatureServiceDisabledFeature(ILogger logger, string featureName, string productName, string ringName)
        {
            _featureServiceDisabled(logger, featureName, productName, ringName, null);
        }
        public static void FeatureServiceToggleIsNotActive(ILogger logger, string featureName, string productName, string ringName, string toggle)
        {
            _toggleIsNotActive(logger, featureName, productName, ringName, toggle, null);
        }
        public static void FeatureServiceProcessingFail(ILogger logger, string featureName, string productName, string ringName, Exception exception)
        {
            _featureServiceThrow(logger, featureName, productName, ringName, exception);
        }
        public static void FeatureServiceProcessingEnd(ILogger logger, string featureName, string productName, string ringName, bool enabled, long elapsedMilliseconds)
        {
            _featureServiceEnd(logger, featureName, productName, ringName, enabled, elapsedMilliseconds, null);
        }
        public static void DefaultToggleTypeActivatorResolveTypeBegin(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorResolveTypeBegin(logger, toggleType, null);
        }
        public static void DefaultToggleTypeActivatorTypeIsResolvedFromCache(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorTypeIsResolvedFromCache(logger, toggleType, null);
        }
        public static void DefaultToggleTypeActivatorTypeIsResolved(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorTypeIsResolved(logger, toggleType, null);
        }
        public static void DefaultToggleTypeActivatorTypeCantResolved(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorCantResolve(logger, toggleType, null);
        }

        private static readonly Action<ILogger, string, string, string, Exception> _featureServiceBegin = LoggerMessage.Define<string, string, string>(
            LogLevel.Debug,
            EventIds.DefaultFeatureServiceBegin,
            "Running DefaultFeatureService to check {featureName}-{productName}-{ringName}.");

        private static readonly Action<ILogger, string, string, string, Exception> _featureServiceFromSession = LoggerMessage.Define<string, string, string>(
            LogLevel.Information,
            EventIds.DefaultFeatureServiceFromSession,
            "DefaultFeatureService use stored session result for {featureName}-{productName}-{ringName}.");

        private static readonly Action<ILogger, string, string, string, bool, long, Exception> _featureServiceEnd = LoggerMessage.Define<string, string, string, bool, long>(
            LogLevel.Debug,
            EventIds.DefaultFeatureServiceEnd,
            "DefaultFeatureService evaluate {featureName}-{productName}-{ringName} with result Enabled:{enabled} on {elapsedMilliseconds} ms.");

        private static readonly Action<ILogger, string, string, string, Exception> _featureServiceNotFound = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotFound,
            "The feature {feature}-{productName}-{ringName} is not configured in the store.");

        private static readonly Action<ILogger, string, string, string, Exception> _featureServiceDisabled = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureDisabled,
            "The feature {feature}-{productName}-{ringName} is disabled on the store.");

        private static readonly Action<ILogger, string, string, string, string, Exception> _toggleIsNotActive = LoggerMessage.Define<string, string, string, string>(
           LogLevel.Debug,
           EventIds.ToggleNotActive,
           "The feature {feature}-{productName}-{ringName} has {toggle} not active.");

        private static readonly Action<ILogger, string, string, string, Exception> _featureServiceThrow = LoggerMessage.Define<string, string, string>(
            LogLevel.Error,
            EventIds.DefaultFeatureServiceThrows,
            "DefaultFeatureService threw an unhandled exception checking {featureName}-{productName}-{ringName}.");

        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorResolveTypeBegin = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.DefaultToggleTypeActivatorResolveTypeBegin,
            "DefaultToggleTypeActivator is trying to resolve the toggle type {toggleType}.");

        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorCantResolve = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.DefaultToggleTypeActivatorCantResolve,
            "DefaultToggleTypeActivator can't resolve the toggle type {toggleType}.");

        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorTypeIsResolvedFromCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.DefaultToggleTypeActivatorTypeIsResolved,
            "DefaultToggleTypeActivator resolve successfully the toggle type {toggleType} from default cache type.");

        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorTypeIsResolved = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.DefaultToggleTypeActivatorTypeIsResolved,
            "DefaultToggleTypeActivator resolve successfully the toggle type {toggleType} creating a type instance.");
    }
}
