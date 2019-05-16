using Microsoft.Extensions.Logging;
using System;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    static class Log
    {
        public static void FindFeature(ILogger logger, string featureName, string productName)
        {
            _findFeature(logger, featureName, productName ?? "(default product)", null);
        }

        public static void FindUserPreviewFeatures(ILogger logger, string productName)
        {
            _findUserPreviewFeatures(logger, productName ?? "(default product)", null);
        }

        public static void EnablingUserOnPreviewFeature(ILogger logger, string featureName, string userName, string productName)
        {
            _enablingUserNameOnPreviewFeature(logger, userName, featureName, productName ?? "(default product)", null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName ?? "(default product)", null);
        }

        public static void FeatureIsNotPreviewOrNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExistOrIsNotPreview(logger, featureName, productName ?? "(default product)", null);
        }

        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for product {productName}.");

        private static readonly Action<ILogger, string, string, Exception> _featureNotExistOrIsNotPreview = LoggerMessage.Define<string, string>(
          LogLevel.Warning,
          EventIds.FeatureIsNotPreviewOrNotExist,
          "The feature with name {featureName} is not a User Preview feature or is not configured for product {productName}.");

        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "Finding feature with name {featureName} for product {productName}.");

        private static readonly Action<ILogger, string, Exception> _findUserPreviewFeatures = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.FindUserPreviewFeatures,
            "Finding User Preview features for product {productName}.");

        private static readonly Action<ILogger, string, string, string, Exception> _enablingUserNameOnPreviewFeature = LoggerMessage.Define<string, string, string>(
            LogLevel.Debug,
            EventIds.EnablingUserOnPreviewFeature,
            "Enabling user {userName} on feature {featureName} for product {productName}.");

    }
}
