using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Configuration.Store.Diagnostics
{
    static class Log
    {
        public static void StoreIsReadOnly(ILogger logger)
        {
            _storeIsReadOnly(logger, nameof(ConfigurationFeatureStore), null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string applicationName)
        {
            _featureNotExist(logger, featureName, applicationName ?? "(default application)", null);
        }

        public static void ParameterNotExist(ILogger logger, string featureName, string applicationName, string parameterName)
        {
            _parameterNotExist(logger, featureName, applicationName ?? "(default application)", parameterName, null);
        }

        public static void FindFeature(ILogger logger, string featureName, string applicationName)
        {
            _findFeature(logger, featureName, applicationName ?? "(default application)", null);
        }

        private static readonly Action<ILogger, string, Exception> _storeIsReadOnly = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.StoreIsReadOnly,
            "Store {configurationStore} is read only, this action can't be performed.");

        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for application {applicationName}.");

        private static readonly Action<ILogger, string, string, string, Exception> _parameterNotExist = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} for application {applicationName} not contains a parameter with name {parameterName} for specified Toggle.");

        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "The store is trying to find feature {featureName} for application {applicationName} on the store.");
    }
}
