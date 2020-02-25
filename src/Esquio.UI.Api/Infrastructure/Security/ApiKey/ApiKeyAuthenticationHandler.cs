using Esquio.UI.Api.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Infrastructure.Security.ApiKey
{
    internal class ApiKeyAuthenticationHandler
        : AuthenticationHandler<ApiKeyOptions>
    {
        private readonly IApiKeyStore _apiKeyStore;

        public ApiKeyAuthenticationHandler(IApiKeyStore apikeyStore, IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
            : base(options, loggerFactory, encoder, clock)
        {
            _apiKeyStore = apikeyStore ?? throw new ArgumentNullException(nameof(apikeyStore));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Log.ApiKeyAuthenticationBegin(Logger);

            var apiKeyValues = this.Context.Request
                .Headers[ApiConstants.ApiKeyHeaderName];

            if (apiKeyValues.Any())
            {
                var selectedApiKey = apiKeyValues.Last();

                try
                {
                    var identity = await _apiKeyStore
                        .ValidateApiKey(apiKey: selectedApiKey, authenticationScheme: Scheme.Name);

                    if (identity != default)
                    {
                        var ticket = new AuthenticationTicket(
                            principal: new ClaimsPrincipal(identity),
                            authenticationScheme: Scheme.Name);

                        Log.ApiKeyAuthenticationSuccess(Logger);
                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        Log.ApiKeyAuthenticationNotFound(Logger, selectedApiKey);
                        return AuthenticateResult.Fail("The api key does not exist in the store.");
                    }

                }
                catch (Exception exception)
                {
                    Log.ApiKeyAuthenticationFail(Logger, exception);
                    return AuthenticateResult.Fail(exception);
                }
            }

            Log.ApiKeyAuthenticationDoesNotExist(Logger);
            return AuthenticateResult.NoResult();
        }
    }
}
