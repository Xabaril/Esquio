using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Esquio.CliTool.Internal
{
    public class EsquioClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private EsquioClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> ListProductsAsync()
        {
            return await _httpClient.GetAsync($"api/v1/products?pageIndex=0&pageCount=99");
        }

        public async Task<HttpResponseMessage> AddProductAsync(string productName, string description)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                Name = productName,
                Description = description,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync("api/v1/products", content);
        }

        public async Task<HttpResponseMessage> RemoveProductAsync(string name)
        {
            return await _httpClient.DeleteAsync($"api/v1/products/{name}");
        }

        public async Task<HttpResponseMessage> ListFeaturesAsync(int productId)
        {
            return await _httpClient.GetAsync($"api/v1/products/{productId}/flags");
        }

        public async Task<HttpResponseMessage> RolloutFeatureAsync(int featureId)
        {
            return await _httpClient.PutAsync($"api/v1/flags/{featureId}/rollout", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> RollbackFeatureAsync(int featureId)
        {
            return await _httpClient.PutAsync($"api/v1/flags/{featureId}/rollback", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> ListTogglesAsync(int featureId)
        {
            return await _httpClient.GetAsync($"api/v1/flags/{featureId}");
        }

        public async Task<HttpResponseMessage> GetToggleAsync(int toggleId)
        {
            return await _httpClient.GetAsync($"api/v1/toggles/{toggleId}");
        }

        public async Task<HttpResponseMessage> SetParameterValue(int toggleId, string name, string value)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                Name = name,
                Value = value,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync($"api/v1/toggles/{toggleId}/parameters",content);
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        public static EsquioClient Create(string uri, string apikey)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uri)
            };

            httpClient.DefaultRequestHeaders.Add("x-api-key", apikey);

            return new EsquioClient(httpClient);
        }
    }
}
