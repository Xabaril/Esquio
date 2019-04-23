using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    static class Log
    {
        public static void FeatureFlagConstraintBegin(ILogger logger, string featureName, string applicationName)
        {
            _featureFlagConstraintBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureFlagConstraintSuccess(ILogger logger, string featureName, string applicationName)
        {
            _featureFlagConstraintSuccess(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureFlagConstraintThrow(ILogger logger, string featureName, string applicationName, Exception exception)
        {
            _featureFlagConstraintThrow(logger, featureName, applicationName ?? "(default application)", exception);
        }
        private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureFlagConstraintBeginProcess,
            "FeatureFlag constraint begin check if {featureName} for application {applicationName} is enabled.");
        private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintSuccess = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureFlagConstraintSuccess,
            "FeatureFlag constraint successfully check if  {featureName} for application {applicationName} is enabled.");
        private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.FeatureFlagConstraintThrow,
            "Feature service throw an error trying to check the feature {featureName} for application {applicationName}.");
    }
}
