using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Diagnostics
{
    static class Log
    {
        public static void FeatureServiceProcessingBegin(ILogger logger, string featureName, string productName)
        {
            _featureServiceBegin(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureServiceNotFoundFeature(ILogger logger, string featureName, string productName)
        {
            _featureServiceNotFound(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureServiceDisabledFeature(ILogger logger, string featureName, string productName)
        {
            _featureServiceNotFound(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureServiceToggleIsNotActive(ILogger logger, string featureName, string productName)
        {
            _featureServiceNotActive(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureServiceProcessingFail(ILogger logger, string featureName, string productName, Exception exception)
        {
            _featureServiceThrow(logger, featureName, productName ?? "(default product)", exception);
        }
        public static void DefaultToggleTypeActivatorResolveTypeBegin(ILogger logger, string toggleType)
        {
            _defaultToggleTypeActivatorResolveTypeBegin(logger, toggleType, null);
        }

        private static readonly Action<ILogger, string, string, Exception> _featureServiceBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.DefaultFeatureServiceBegin,
            "Running DefaultFeatureService to check {featureName} for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureServiceNotFound = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotFound,
            "The feature {feature} is not configured in the store for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureServiceDisabled = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureDisabled,
            "The feature {feature} is disabled in the store for application {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureServiceNotActive = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureNotActive,
           "The feature {feature} is not active on product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureServiceThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.DefaultFeatureServiceThrows,
            "DefaultFeatureService threw an unhandled exception checking {featureName} for product {productName}.");
        private static readonly Action<ILogger, string, Exception> _defaultToggleTypeActivatorResolveTypeBegin = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.DefaultToggleTypeActivatorResolveTypeBegin,
            "DefaultToggleTypeActivator is trying to resolve the toggle type {toggleType}.");
    }
}
