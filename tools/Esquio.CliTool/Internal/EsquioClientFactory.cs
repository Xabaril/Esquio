using System;
using System.Net.Http;

namespace Esquio.CliTool.Internal
{
    public sealed class EsquioClientFactory
    {
        private static readonly Lazy<EsquioClientFactory> _lazy =
            new Lazy<EsquioClientFactory>(() => new EsquioClientFactory());
        private readonly HttpClient _httpClient;

        public static EsquioClientFactory Instance => _lazy.Value;

        private EsquioClientFactory()
        {
            _httpClient = new HttpClient();
        }

        public EsquioClient Create(string uri, string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-api-version", "2.0");
            
            return new EsquioClient(uri ?? Constants.UriDefaultValue, _httpClient);
        }
    }
}
