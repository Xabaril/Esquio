using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Services
{
    public interface IEsquioService
    {
        Task GetProducts();
    }

    public class EsquioService
        : IEsquioService
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public EsquioService(HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        private async Task EnsureAuthorization()
        {
            const string AUTHORIZATION_HEADER_NAME = "Authorization";

            if (!_httpClient.DefaultRequestHeaders.TryGetValues(AUTHORIZATION_HEADER_NAME, out _))
            {
                var tokenResult = await _tokenProvider.RequestAccessToken();

                if (tokenResult.Status == AccessTokenResultStatus.Success)
                {
                    if (tokenResult.TryGetToken(out AccessToken token))
                    {
                        _httpClient.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, $"Bearer {token}");
                    }
                }
            }
        }

        public async Task GetProducts()
        {
            await EnsureAuthorization();

            //TODO: implementar get products
            await Task.CompletedTask;
        }
    }
}
