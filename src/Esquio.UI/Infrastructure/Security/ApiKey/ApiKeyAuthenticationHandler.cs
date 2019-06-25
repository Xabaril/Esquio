using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Esquio.UI.Infrastructure.Security.ApiKey
{
    internal class ApiKeyAuthenticationHandler
        : AuthenticationHandler<ApiKeyOptions>
    {
        const string APIKEY_QUERY_NAME = "apikey";
        private readonly IApiKeyStore _apiKeyStore;

        //public ApiKeyAuthenticationHandler(IApiKeyStore apikeyStore, IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
        //    : base(options, loggerFactory, encoder, clock)
        //{
        //    _apiKeyStore = apikeyStore ?? throw new ArgumentNullException(nameof(apikeyStore));
        //}
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
            : base(options, loggerFactory, encoder, clock)
        {
            //_apiKeyStore = apikeyStore ?? throw new ArgumentNullException(nameof(apikeyStore));
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKeyValues = this.Context.Request.Query[APIKEY_QUERY_NAME];

            if (apiKeyValues.Any())
            {
                try
                {
                    var identity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,"luru")
                    });//_apiKeyStore.ValidateApiKey(apiKeyValues.Last(), Scheme.Name);

                    var ticket = new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        authenticationScheme: Scheme.Name);

                    return Task.FromResult<AuthenticateResult>(AuthenticateResult.Success(ticket));
                }
                catch (Exception exception)
                {
                    return Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail(exception));
                }
                
            }
            return Task.FromResult<AuthenticateResult>(AuthenticateResult.NoResult());
        }
    }
}
