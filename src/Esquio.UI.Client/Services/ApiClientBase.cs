using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Services
{
    public abstract class ApiClientBase
    {
        protected readonly ApiConfiguration _configuration;

        protected ApiClientBase(ApiConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_configuration.HttpClient);
        }

        protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            var token = await GetTokenAsync();

            if (token != null)
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            return msg;
        }

        private async Task<AccessToken> GetTokenAsync()
        {
            const string AUTHORIZATION_HEADER_NAME = "Authorization";

            if (!_configuration.HttpClient.DefaultRequestHeaders.TryGetValues(AUTHORIZATION_HEADER_NAME, out _))
            {
                var tokenResult = await _configuration.AccessTokenProvider.RequestAccessToken();

                if (tokenResult.Status == AccessTokenResultStatus.Success)
                {
                    if (tokenResult.TryGetToken(out AccessToken token))
                    {
                        return token;
                    }
                }
            }

            return null;
        }
    }
}
