using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Net.Http;

namespace Esquio.UI.Client.Services
{
    public class ApiConfiguration
    {
        public ApiConfiguration(HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            AccessTokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        public HttpClient HttpClient { get; }

        public IAccessTokenProvider AccessTokenProvider { get; }
    }
}
