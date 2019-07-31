using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Diagnostics
{
    static class Log
    {
        public static void FeatureServiceProcessingBegin(ILogger logger, string featureName, string productName)
        {
            _featureServiceBegin(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureServiceNotFoundFeature(ILogger logger, string featureName, string productName)
        {
            _featureServiceNotFound(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureServiceDisabledFeature(ILogger logger, string featureName, string productName)
        {
            _featureServiceDisabled(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureServiceToggleIsNotActive(ILogger logger, string toggle, string featureName)
        {
            _toggleIsNotActive(logger, toggle, featureName, null);
        }
        public static void FeatureServiceProcessingFail(ILogger logger, string featureName, string productName, Exception exception)
        {
            _featureServiceThrow(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, exception);
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

        private static readonly Action<ILogger, string, string, Exception> _toggleIsNotActive = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.ToggleNotActive,
           "The toggle {toggle} on feature {feature} is not active.");

        private static readonly Action<ILogger, string, string, Exception> _featureServiceThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.DefaultFeatureServiceThrows,
            "DefaultFeatureService threw an unhandled exception checking {featureName} for product {productName}.");

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
