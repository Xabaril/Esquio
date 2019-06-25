using Microsoft.Extensions.Logging;
using System;

namespace Esquio.UI.Api.Diagnostics
{
    static class Log
    {
        public static void ApiKeyAuthenticationBegin(ILogger logger)
        {
            _apiKeyAuthenticationBegin(logger, null);
        }

        public static void ApiKeyAuthenticationDoesNotExist(ILogger logger)
        {
            _apiKeyAuthenticatioDoesNotExist(logger, null);
        }

        public static void ApiKeyAuthenticationSuccess(ILogger logger)
        {
            _apiKeyAuthenticationSucess(logger, null);
        }

        public static void ApiKeyAuthenticationNotFound(ILogger logger, string apiKey)
        {
            _apiKeyAuthenticationNotFound(logger, apiKey, null);
        }

        public static void ApiKeyAuthenticationFail(ILogger logger, Exception exception)
        {
            _apiKeyAuthenticationFail(logger, exception);
        }

        public static void ApiKeyStoreValidating(ILogger logger)
        {
            _apiKeyStoreValidating(logger, null);
        }

        public static void ApiKeyStoreKeyExist(ILogger logger)
        {
            _apiKeyStoreKeyExist(logger, null);
        }

        private static readonly Action<ILogger, Exception> _apiKeyAuthenticationBegin = LoggerMessage.Define(
            LogLevel.Information,
            EventIds.ApiKeyAuthenticationBegin,
            "The ApiKey authentication handler is handling authentication.");

        private static readonly Action<ILogger, Exception> _apiKeyAuthenticatioDoesNotExist = LoggerMessage.Define(
            LogLevel.Debug,
            EventIds.ApiKeyAuthenticationDoesNotExist,
            "The ApiKey authentication handler result as NoResult because no api key headers can't be used.");

        private static readonly Action<ILogger, Exception> _apiKeyAuthenticationSucess = LoggerMessage.Define(
            LogLevel.Information,
            EventIds.ApiKeyAuthenticationSuccess,
            "The ApiKey authentication success.");

        private static readonly Action<ILogger, string, Exception> _apiKeyAuthenticationNotFound = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.ApiKeyAuthenticationNotFound,
            "The ApiKey {apiKey} is not present on valid api key list.");

        private static readonly Action<ILogger, Exception> _apiKeyAuthenticationFail = LoggerMessage.Define(
          LogLevel.Error,
          EventIds.ApiKeyAuthenticationFail,
          "The ApiKey authentication fail.");

        private static readonly Action<ILogger, Exception> _apiKeyStoreValidating = LoggerMessage.Define(
           LogLevel.Debug,
           EventIds.ApiKeyStoreValidating,
           "Validating the api key *** on the store.");

        private static readonly Action<ILogger, Exception> _apiKeyStoreKeyExist = LoggerMessage.Define(
          LogLevel.Information,
          EventIds.ApiKeyStoreKeyExist,
          "The api key *** exist on the store. A new identity is created using this information.");
    }
}
