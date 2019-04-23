using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Diagnostics
{
    static class Log
    {
        public static void FeatureServiceProcessingBegin(ILogger logger, string featureName, string applicationName)
        {
            _featureServiceBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureServiceNotFoundFeature(ILogger logger, string featureName, string applicationName)
        {
            _featureServiceNotFound(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureServiceToggleIsNotActive(ILogger logger, string featureName, string applicationName)
        {
            _featureServiceNotActive(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureServiceProcessingFail(ILogger logger, string featureName, string applicationName, Exception exception)
        {
            _featureServiceThrow(logger, featureName, applicationName ?? "(default application)", exception);
        }
        public static void DefaultToggleTypeActivatorResolveTypeBegin(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorResolveTypeBegin(logger, toggleType, null);
        }

        private static readonly Action<ILogger, string, string, Exception> _featureServiceBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.DefaultFeatureServiceBegin,
            "Running DefaultFeatureService to check {featureName} for application {applicationName}.");

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
            "DefaultFeatureService threw an unhandled exception checking {featureName} for application {application}.");

        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorResolveTypeBegin = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.DefaultToggleTypeActivatorResolveTypeBegin,
            "DefaultToggleTypeActivator is trying to resolve the toggle type {toggleType}.");
    }
}
